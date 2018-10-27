using AutoMapper;
using DrawManager.Api.Entities;
using DrawManager.Api.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Users
{
    public class Login
    {
        public class UserData
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        public class Command : IRequest<UserEnvelope>
        {
            public UserData UserData { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.UserData).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerDbContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;

            public Handler(IMapper mapper, DrawManagerDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
            {
                _mapper = mapper;
                _context = context;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public async Task<UserEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                // Getting user from db
                var user = await _context
                    .Users
                    .Where(u => u.Login == request.UserData.Login)
                    .SingleOrDefaultAsync(cancellationToken);

                // Validations
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                }

                if (!user.Hash.SequenceEqual(_passwordHasher.Hash(request.UserData.Password, user.Salt)))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                }

                // Mapping
                var userEnvelope = _mapper.Map<User, UserEnvelope>(user);
                userEnvelope.Token = await _jwtTokenGenerator.CreateToken(user.Login);
                return userEnvelope;
            }
        }
    }
}
