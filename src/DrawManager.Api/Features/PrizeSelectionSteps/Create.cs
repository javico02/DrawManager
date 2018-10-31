using AutoMapper;
using DrawManager.Api.Entities;
using DrawManager.Api.Features.Prizes;
using DrawManager.Api.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.PrizeSelectionSteps
{
    public class Create
    {
        public class Command : IRequest<PrizeSelectionStepEnvelope>
        {
            public int PrizeId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PrizeId)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, PrizeSelectionStepEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerDbContext _context;
            private readonly IRandomSelector _randomSelector;

            public Handler(IMapper mapper, DrawManagerDbContext context, IRandomSelector randomSelector)
            {
                _mapper = mapper;
                _context = context;
                _randomSelector = randomSelector;
            }

            public async Task<PrizeSelectionStepEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                // Getting the parent prize & your prizes selection steps
                var prize = await _context
                    .Prizes
                    .Include(p => p.Draw)
                    .Include(p => p.SelectionSteps)
                    .FirstOrDefaultAsync(d => d.Id == request.PrizeId, cancellationToken);

                // Validations
                if (prize == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El premio con id '{ request.PrizeId}' no existe." });
                }

                // If prize was delivered, then it is removed the winner existent step for creating a new one
                if (prize.Delivered)
                {
                    // Getting winner step
                    var winnerStep = prize
                        .SelectionSteps
                        .FirstOrDefault(pss => pss.PrizeSelectionStepType == PrizeSelectionStepType.Winner);

                    if (winnerStep == null)
                        throw new RestException(HttpStatusCode.BadRequest, new { Error = $"El premio con nombre '{ prize.Name }' ya ha sido entregado pero no existe paso ganador." });

                    // Deleting winner step
                    _context.PrizeSelectionSteps.Remove(winnerStep);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // Selecting loser steps inside prize
                var previousLosers = prize
                    .SelectionSteps
                    .Where(st => st.PrizeSelectionStepType == PrizeSelectionStepType.Loser)
                    .ToList();

                // Getting all entrants for the draw & prize excluding the losers steps
                // TODO: Get all winner steps from previous draws except from the car draw for excluding.
                var allEntrantsExcludingLosers = await _context
                    .DrawEntries
                    .Include(de => de.Entrant)
                    .Where(de => de.DrawId == prize.DrawId && previousLosers.All(l => l.EntrantId != de.EntrantId))
                    .ToListAsync();

                // Selecting entry
                var drawEntry = _randomSelector
                    .TakeRandom(allEntrantsExcludingLosers, 1, allEntrantsExcludingLosers.Count)
                    .Single();

                // Creating prize
                var prizeSelectionStep = new PrizeSelectionStep
                {
                    PrizeId = prize.Id,
                    EntrantId = drawEntry.EntrantId,
                    PrizeSelectionStepType = (previousLosers.Count < prize.AttemptsUntilChooseWinner)
                                                ? PrizeSelectionStepType.Loser
                                                : PrizeSelectionStepType.Winner,
                    RegisteredOn = DateTime.Now,
                };

                // Updating prize
                if (prizeSelectionStep.PrizeSelectionStepType == PrizeSelectionStepType.Winner)
                {
                    prize.ExecutedOn = DateTime.Now;

                    // Loading all draw info
                    var draw = await _context
                    .Draws
                    .Include(d => d.Prizes)
                        .ThenInclude(p => p.SelectionSteps)
                    .FirstOrDefaultAsync(d => d.Id == prize.DrawId, cancellationToken);

                    // Updating 
                    var mustBeCompleted = draw
                        .Prizes
                        .Where(p => p.Id != prize.Id)
                        .All(p => p.Delivered);

                    if (mustBeCompleted)
                        draw.ExecutedOn = DateTime.Now;
                }

                // Saving
                await _context.PrizeSelectionSteps.AddAsync(prizeSelectionStep, cancellationToken);
                prize.SelectionSteps.Add(prizeSelectionStep);
                await _context.SaveChangesAsync(cancellationToken);

                // Mapping
                var prizeSelectionStepEnvelope = _mapper.Map<PrizeSelectionStep, PrizeSelectionStepEnvelope>(prizeSelectionStep);
                return prizeSelectionStepEnvelope;
            }
        }
    }
}
