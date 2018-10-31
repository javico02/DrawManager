using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Prizes
{
    [Route("draws")]
    [ApiController]
    public class PrizesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrizesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/prizes")]
        public async Task<PrizesEnvelope> Get([FromRoute]int id)
        {
            return await _mediator.Send(new List.Query(id));
        }

        [HttpPost("{id}/prizes")]
        public async Task<PrizeEnvelope> Create([FromRoute]int id, [FromBody]Create.PrizeData prizeData)
        {
            return await _mediator.Send(new Create.Command {
                DrawId = id,
                Prize = prizeData
            });
        }

        [HttpDelete("{id}/prizes/{prizeId}")]
        public async Task Delete([FromRoute]int id, [FromRoute]int prizeId)
        {
            await _mediator.Send(new Delete.Command(id, prizeId));
        }
    }
}