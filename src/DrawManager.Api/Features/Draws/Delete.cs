using DrawManager.Api.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Draws
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int DrawId { get; private set; }

            public Command(int drawId)
            {
                DrawId = drawId;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.DrawId)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DrawManagerDbContext _context;

            public Handler(DrawManagerDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Validation
                var draw = await _context
                    .Draws
                    .FirstOrDefaultAsync(d => d.Id == request.DrawId, cancellationToken);
                if (draw == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El sorteo con id '{request.DrawId}' no existe." });

                // Saving
                _context.Draws.Remove(draw);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
