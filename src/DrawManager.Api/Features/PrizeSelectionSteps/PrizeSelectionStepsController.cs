using DrawManager.Api.Features.Prizes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.PrizeSelectionSteps
{
    [Route("prizes")]
    [ApiController]
    public class PrizeSelectionStepsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrizeSelectionStepsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{id}/prizeSelectionSteps")]
        public async Task<PrizeSelectionStepEnvelope> Create([FromRoute]int id)
        {

            return await _mediator.Send(new Create.Command
            {
                PrizeId = id
            });
        }
    }
}