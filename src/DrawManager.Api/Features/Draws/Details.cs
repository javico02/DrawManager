using AutoMapper;
using DrawManager.Api.Entities;
using DrawManager.Api.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Draws
{
    public class Details
    {
        public class Query : IRequest<DrawEnvelope>
        {
            public int DrawId { get; private set; }

            public Query(int drawId)
            {
                DrawId = drawId;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.DrawId)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public class QueryHandler : IRequestHandler<Query, DrawEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerDbContext _context;

            public QueryHandler(IMapper mapper, DrawManagerDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<DrawEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                // Getting draw
                var draw = await _context
                    .Draws
                    .Include(d => d.Prizes)
                        .ThenInclude(p => p.SelectionSteps)
                    .Include(d => d.Entries)
                    .FirstOrDefaultAsync(d => d.Id == request.DrawId, cancellationToken);

                // Validations
                if (draw == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El sorteo con el id '{ request.DrawId }' no existe." });
                }

                // Loading entrant info 
                var allPrizeSelectionSteps = draw
                    .Prizes
                    .SelectMany(p => p.SelectionSteps);
                foreach (var pss in allPrizeSelectionSteps)
                {
                    await _context
                        .Entry(pss)
                        .Reference(p => p.Entrant)
                        .LoadAsync();
                }

                // Mapping
                var drawEnvelope = _mapper.Map<Draw, DrawEnvelope>(draw);
                return drawEnvelope;
            }
        }
    }
}
