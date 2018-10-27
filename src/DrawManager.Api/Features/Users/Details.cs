using AutoMapper;
using DrawManager.Api.Entities;
using DrawManager.Api.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Users
{
    public class Details
    {
        public class Query : IRequest<UserEnvelope>
        {
            public string Login { get; private set; }

            public Query(string login)
            {
                Login = login;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.Login)
                    .NotNull()
                    .NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, UserEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerDbContext _context;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;

            public QueryHandler(DrawManagerDbContext context, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public async Task<UserEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                // Getting user
                var user = await _context
                    .Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Login.Equals(message.Login, StringComparison.InvariantCultureIgnoreCase), cancellationToken);

                // Validations
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El usuario '{message.Login}' no existe." });
                }

                // Mapping
                var userEnvelope = _mapper.Map<User, UserEnvelope>(user);
                userEnvelope.Token = await _jwtTokenGenerator.CreateToken(user.Login);
                return userEnvelope;
            }
        }
    }
}
