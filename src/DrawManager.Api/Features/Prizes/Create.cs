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

namespace DrawManager.Api.Features.Prizes
{
    public class Create
    {
        public class PrizeData
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int AttemptsUntilChooseWinner { get; set; }
        }

        public class PrizeDataValidator : AbstractValidator<PrizeData>
        {
            public PrizeDataValidator()
            {
                RuleFor(pd => pd.Name)
                    .NotNull()
                    .NotEmpty();

                RuleFor(pd => pd.Description)
                    .NotNull()
                    .NotEmpty();

                RuleFor(pd => pd.AttemptsUntilChooseWinner)
                    .NotNull()
                    .InclusiveBetween(0, 5);
            }
        }

        public class Command : IRequest<PrizeEnvelope>
        {
            public int DrawId { get; set; }
            public PrizeData Prize { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.DrawId)
                    .NotNull()
                    .GreaterThan(0);

                RuleFor(c => c.Prize)
                    .SetValidator(new PrizeDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, PrizeEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerDbContext _context;

            public Handler(IMapper mapper, DrawManagerDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<PrizeEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                // Getting the parent draw & your prizes
                var draw = await _context
                    .Draws
                    .Include(d => d.Prizes)
                    .FirstOrDefaultAsync(d => d.Id == request.DrawId, cancellationToken);

                // Validations
                if (draw == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El sorteo con id '{ request.DrawId }' no existe." });
                }

                var existPrize = draw.Prizes.Any(p => p.Name.Equals(request.Prize.Name, StringComparison.InvariantCultureIgnoreCase));
                if (existPrize)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Error = $"El premio con nombre '{ request.Prize.Name }' ya existe." });
                }


                // Creating prize
                var prize = new Prize
                {
                    DrawId = request.DrawId,
                    Name = request.Prize.Name,
                    Description = request.Prize.Description,
                    AttemptsUntilChooseWinner = request.Prize.AttemptsUntilChooseWinner,
                };

                // Saving
                await _context.Prizes.AddAsync(prize, cancellationToken);
                draw.Prizes.Add(prize);
                await _context.SaveChangesAsync(cancellationToken);

                // Mapping
                var prizeEnvelope = _mapper.Map<Prize, PrizeEnvelope>(prize);
                return prizeEnvelope;
            }
        }
    }
}
