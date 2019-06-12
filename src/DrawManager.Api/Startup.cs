using AutoMapper;
using DrawManager.Api;
using DrawManager.Api.Infrastructure;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace DrawManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registering MediatR options
            services.AddMediatR();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            // Registering database context
            var dbConnString = Environment.GetEnvironmentVariable(DrawManagerApiWellKnownConstants.DB_CONNECTIONSTRING_KEY)
                ?? Configuration.GetConnectionString(DrawManagerApiWellKnownConstants.DB_CONNECTIONSTRING_KEY);
            services
                .AddDbContext<DrawManagerDbContext>(options => options.UseSqlite(dbConnString));

            // Registering swagger options
            var swaggerVersion = Configuration[DrawManagerApiWellKnownConstants.SWAGGER_VERSION_KEY];
            var swaggerTitle = Configuration[DrawManagerApiWellKnownConstants.SWAGGER_TITLE_KEY];
            var swaggerDescription = Configuration[DrawManagerApiWellKnownConstants.SWAGGER_DESCRIPTION_KEY];
            var swaggerTermsOfService = Configuration[DrawManagerApiWellKnownConstants.SWAGGER_TERMSOFSERVICE_KEY];
            services
                .AddSwaggerGen(x =>
                {
                    x.SwaggerDoc(swaggerVersion, new Info
                    {
                        Title = swaggerTitle,
                        Version = swaggerVersion,
                        Description = swaggerDescription,
                        TermsOfService = swaggerTermsOfService
                    });
                    x.CustomSchemaIds(y => y.FullName);
                    x.DocInclusionPredicate((version, apiDescription) => true);
                    x.TagActionsBy(y => y.GroupName);
                });

            // Registering Cors
            services.AddCors();

            // Registering Mvc options
            services
                .AddMvc(options =>
                {
                    options.Conventions.Add(new GroupByApiRootConvention());
                    options.Filters.Add(typeof(ValidatorActionFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Registering AutoMapper
            services.AddAutoMapper(GetType().Assembly);

            // Registering services
            services.AddScoped<IRandomSelector, RandomSelector>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Registering Jwt
            services.AddJwt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Enable database creation ensuring and migrations
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<DrawManagerDbContext>().Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable Serilog
            loggerFactory.AddSerilogLogging();

            // Enable error's middleware
            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Enable Cors
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            // Enable Mvc
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(Configuration[DrawManagerApiWellKnownConstants.SWAGGER_ENDPOINT_KEY], Configuration[DrawManagerApiWellKnownConstants.SWAGGER_NAME_KEY]);
            });
        }
    }
}
