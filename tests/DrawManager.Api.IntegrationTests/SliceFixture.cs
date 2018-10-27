using DrawManager.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DrawManager.Api.IntegrationTests
{
    public class SliceFixture : IDisposable
    {
        private readonly string _dbConnString;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SliceFixture()
        {
            // Getting configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            _dbConnString = configuration.GetConnectionString(DrawManagerApiWellKnownConstants.DB_CONNECTIONSTRING_KEY);

            // Configuring services
            var services = new ServiceCollection();
            services.AddDbContext<DrawManagerDbContext>(options => options.UseSqlite(_dbConnString));

            // Creating startup
            var startup = new Startup(configuration);
            startup.ConfigureServices(services);

            // Creating database
            var provider = services.BuildServiceProvider();
            var dbContext = provider.GetRequiredService<DrawManagerDbContext>();
            dbContext.Database.Migrate();

            //Initializing 
            _serviceScopeFactory = provider.GetService<IServiceScopeFactory>();
        }

        public void Dispose()
        {
            if (File.Exists(_dbConnString))
                File.Delete(_dbConnString);
        }
    }
}

