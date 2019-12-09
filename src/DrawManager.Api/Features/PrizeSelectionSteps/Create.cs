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
                // Getting the parent prize & prizes selection steps
                var prize = await _context
                    .Prizes
                    .Include(p => p.Draw)
                    .Include(p => p.SelectionSteps)
                    .FirstOrDefaultAsync(d => d.Id == request.PrizeId, cancellationToken);

                // Validating prize's existence
                if (prize == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El premio con id '{ request.PrizeId}' no existe." });
                }

                // Validating if the draw is already closed.
                if (prize.Draw.ExecutedOn.HasValue)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Error = $"El sorteo '{ prize.Draw.Name }' se encuentra cerrado desde el { prize.Draw.ExecutedOn }." });
                }

                // If prize was delivered, then it is removed the winner existent step for creating a new one
                int removedEntrantId = -1;
                if (prize.Delivered)
                {
                    // Getting winner step
                    var winnerStep = prize
                        .SelectionSteps
                        .FirstOrDefault(pss => pss.PrizeSelectionStepType == PrizeSelectionStepType.Winner);

                    if (winnerStep == null)
                        throw new RestException(HttpStatusCode.BadRequest, new { Error = $"El premio con nombre '{ prize.Name }' ya ha sido entregado pero no existe paso ganador." });

                    // Getting entrant id for winner step to remove
                    removedEntrantId = winnerStep.EntrantId;

                    // Deleting winner step
                    _context.PrizeSelectionSteps.Remove(winnerStep);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // Selecting loser steps inside prize
                var previousLoserSteps = prize
                    .SelectionSteps
                    .Where(st => st.PrizeSelectionStepType == PrizeSelectionStepType.Loser)
                    .ToList();

                // Selecting all winners from all prizes that belongs to draw's same group.
                var previousWinnerSteps = await _context
                    .PrizeSelectionSteps
                    .Include(pss => pss.Prize)
                        .ThenInclude(p => p.Draw)
                    .Include(pss => pss.Entrant)
                    .Where(pss => pss.PrizeSelectionStepType == PrizeSelectionStepType.Winner && pss.Prize.Draw.GroupName == prize.Draw.GroupName)
                    .ToListAsync(cancellationToken);

                // Getting all entries for the draw & prize.
                var allEntries = _context
                    .GetEntriesByDrawExcludingPreviousWinnersAndLosers(prize.DrawId);

                // Excluding the losers steps for the current prize, the prize's winners from others draws that belongs to the same group and the removed entrant id
                var entries = removedEntrantId ==- -1 
                    ? allEntries
                        .Where(de =>
                            previousLoserSteps.All(l => l.EntrantId != de.EntrantId)
                            && previousWinnerSteps.All(w => w.EntrantId != de.EntrantId)
                        )
                        .ToList()
                    : allEntries
                        .Where(de =>
                            removedEntrantId != de.EntrantId
                            && previousLoserSteps.All(l => l.EntrantId != de.EntrantId)
                            && previousWinnerSteps.All(w => w.EntrantId != de.EntrantId)
                        )
                        .ToList();

                // Validating the existence of entrants for the draw.
                if (entries.Count == 0)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Error = $"No existen participantes disponibles en el sorteo '{ prize.Draw.Name }' para seleccionar un ganador." });
                }

                // Selecting winner entry
                var selectedEntry = _randomSelector
                    .TakeRandom(entries, 1, entries.Count)
                    .SingleOrDefault();

                // Validating entry's selection existence
                if (selectedEntry == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = "No fue seleccionado ninguna participación." });
                }

                // Getting the whole info of the winner entry
                var winnerEntry = _context
                    .DrawEntries
                    .Include(de => de.Entrant)
                    .SingleOrDefault(de => de.Id == selectedEntry.Id);

                // Creating prize selection step
                var prizeSelectionStep = new PrizeSelectionStep
                {
                    PrizeId = prize.Id,
                    EntrantId = winnerEntry.EntrantId,
                    DrawEntryId = winnerEntry.Id,
                    PrizeSelectionStepType = (previousLoserSteps.Count < prize.AttemptsUntilChooseWinner)
                                                ? PrizeSelectionStepType.Loser
                                                : PrizeSelectionStepType.Winner,
                    RegisteredOn = DateTime.Now,
                };

                // Updating prize
                if (prizeSelectionStep.PrizeSelectionStepType == PrizeSelectionStepType.Winner)
                {
                    prize.ExecutedOn = DateTime.Now;
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
