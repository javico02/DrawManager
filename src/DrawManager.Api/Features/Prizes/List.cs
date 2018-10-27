using AutoMapper;
using DrawManager.Api.Entities;
using DrawManager.Api.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Prizes
{
    public class List
    {
        public class Query : IRequest<PrizesEnvelope>
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

        public class QueryHandler : IRequestHandler<Query, PrizesEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerDbContext _context;

            public QueryHandler(IMapper mapper, DrawManagerDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<PrizesEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                // Getting the parent draw & your prizes
                var draw = await _context
                    .Draws
                    .Include(d => d.Prizes)
                        .ThenInclude(p => p.SelectionSteps)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Id == request.DrawId, cancellationToken);

                // Validations
                if (draw == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El sorteo con id '{ request.DrawId }' no existe." });
                }

                // Mapping
                return new PrizesEnvelope
                {
                    Prizes = _mapper.Map<List<Prize>, List<PrizeEnvelope>>(draw.Prizes),
                    PrizesQty = draw.PrizesQty
                };
            }
        }
    }
}
