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

namespace DrawManager.Api.Features.Draws
{
    public class Create
    {
        public class DrawData
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool AllowMultipleParticipations { get; set; }
            public DateTime ProgrammedFor { get; set; }
        }

        public class Command : IRequest<DrawEnvelope>
        {
            public DrawData DrawData { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.DrawData).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, DrawEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerDbContext _context;

            public Handler(IMapper mapper, DrawManagerDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<DrawEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                // Validations
                var exist = await _context
                    .Draws
                    .Where(d => d.Name.Equals(request.DrawData.Name, StringComparison.InvariantCultureIgnoreCase) && d.ProgrammedFor == request.DrawData.ProgrammedFor)
                    .AnyAsync(cancellationToken);
                if (exist)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Error = $"Ya existe un sorteo con nombre '{request.DrawData.Name}' programado para el '{request.DrawData.ProgrammedFor.ToShortDateString()}'" });
                }

                // Creating draw
                var draw = new Draw
                {
                    Name = request.DrawData.Name,
                    Description = request.DrawData.Description,
                    AllowMultipleParticipations = request.DrawData.AllowMultipleParticipations,
                    ProgrammedFor = request.DrawData.ProgrammedFor
                };

                // Saving
                _context.Draws.Add(draw);
                await _context.SaveChangesAsync(cancellationToken);

                // Mapping
                var drawEnvelope = _mapper.Map<Draw, DrawEnvelope>(draw);
                return drawEnvelope;
            }
        }
    }
}
