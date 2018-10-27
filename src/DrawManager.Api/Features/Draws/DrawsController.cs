﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Draws
{
    [Route("draws")]
    [ApiController]
    public class DrawsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DrawsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<DrawsEnvelope> Get([FromQuery]int? limit, [FromQuery]int? offset)
        {
            return await _mediator.Send(new List.Query(limit, offset));
        }

        [HttpGet("{id}")]
        public async Task<DrawEnvelope> Get([FromRoute]int id)
        {
            return await _mediator.Send(new Details.Query(id));
        }

        [HttpPost]
        public async Task<DrawEnvelope> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task Delete([FromRoute]int id)
        {
            await _mediator.Send(new Delete.Command(id));
        }
    }
}