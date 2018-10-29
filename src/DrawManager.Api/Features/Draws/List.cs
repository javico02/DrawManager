using AutoMapper;
using DrawManager.Api.Entities;
using DrawManager.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Draws
{
    public class List
    {
        public class Query : IRequest<DrawsEnvelope>
        {
            public int? Limit { get; private set; }
            public int? Offset { get; private set; }

            public Query(int? limit, int? offset)
            {
                Limit = limit;
                Offset = offset;
            }
        }

        public class QueryHandler : IRequestHandler<Query, DrawsEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerDbContext _context;

            public QueryHandler(IMapper mapper, DrawManagerDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<DrawsEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                // Getting draws that have not been executed yet.
                var draws = await _context
                    .Draws
                    .Include(d => d.Prizes)
                    .Include(d => d.Entries)
                    .Where(d => !d.ExecutedOn.HasValue)
                    .Skip(request.Offset ?? 0)
                    .Take(request.Limit ?? 10)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                // Mapping
                return new DrawsEnvelope
                {
                    Draws = _mapper.Map<List<Draw>, List<DrawEnvelope>>(draws),
                    DrawsCount = await _context.Draws.CountAsync(d => !d.ExecutedOn.HasValue, cancellationToken)
                };
            }
        }
    }
}
