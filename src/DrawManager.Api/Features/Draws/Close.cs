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

namespace DrawManager.Api.Features.Draws
{
    public class Close
    {
        public class Command : IRequest<DrawEnvelope>
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

        public class Handler : IRequestHandler<Command, DrawEnvelope>
        {
            private readonly IMapper _mappper;
            private readonly DrawManagerDbContext _context;

            public Handler(IMapper mapper, DrawManagerDbContext context)
            {
                _mappper = mapper;
                _context = context;
            }

            public async Task<DrawEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                // Getting draw
                var draw = await _context
                    .Draws
                    .Include(d => d.Prizes)
                        .ThenInclude(p => p.SelectionSteps)
                    .FirstOrDefaultAsync(d => d.Id == request.DrawId, cancellationToken);

                // Validations
                if (draw == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El sorteo con id '{ request.DrawId }' no existe." });
                }

                if (draw.ExecutedOn.HasValue)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Error = $"El sorteo '{ draw.Name }' se encuentra cerrado desde el { draw.ExecutedOn }." });
                }

                if (!draw.IsCompleted)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Error = $"El sorteo '{ draw.Name }' no está completado aún. Faltan ganadores por seleccionar." });
                }

                // Closing
                draw.ExecutedOn = DateTime.Now;

                // Saving
                _context.Update(draw);
                await _context.SaveChangesAsync(cancellationToken);

                // Mapping
                var drawEnvelope = _mappper.Map<Draw, DrawEnvelope>(draw);

                // Getting quantity of entries for the draw.
                drawEnvelope.EntriesQty = await _context.DrawEntries.CountAsync(de => de.DrawId == drawEnvelope.Id, cancellationToken);

                return drawEnvelope;
            }
        }
    }
}
