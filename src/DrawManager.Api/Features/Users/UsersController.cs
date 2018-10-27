using DrawManager.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Users
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UsersController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet("current")]
        public async Task<UserEnvelope> Get()
        {
            return await _mediator.Send(new Details.Query(_currentUserAccessor.GetCurrentUsername()));
        }

        [HttpPost("registerNew")]
        public async Task<UserEnvelope> Create([FromBody] Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("login")]
        public async Task<UserEnvelope> Login([FromBody] Login.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}