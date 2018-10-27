using DrawManager.Api.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Prizes
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int DrawId { get; private set; }
            public int PrizeId { get; private set; }

            public Command(int drawId, int prizeId)
            {
                DrawId = drawId;
                PrizeId = prizeId;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.DrawId)
                    .NotNull()
                    .GreaterThan(0);

                RuleFor(c => c.PrizeId)
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
                // Getting prize
                var prize = await _context
                    .Prizes
                    .FirstOrDefaultAsync(p => p.Id == request.PrizeId, cancellationToken);

                // Validation
                if (prize == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El premio con id '{request.PrizeId}' no existe." });
                }

                if (prize.DrawId != request.DrawId)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Error = $"El premio con id '{request.PrizeId}' no está asociado al sorteo con id '{request.DrawId}'." });
                }

                // Saving
                _context.Prizes.Remove(prize);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
