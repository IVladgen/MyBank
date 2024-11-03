using MediatR;
using Microsoft.AspNetCore.Mvc;

using MyBank.Domain.Response;
using MyBank.Domain.Response.Token;
using MyBank.Domain.DTO;
using MyBank.Application.Commands.Token;

namespace MyBank.API.Controllers
{
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ISender _mediator;

        public TokenController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/Auth")]
        public async Task<BaseResponse<RefreshResponse>> RefreshAccessToken(TokenDto token)
        {
            var command = new RefreshTokenCommand()
            {
                dto = token
            };

            var result = await _mediator.Send(command);
            return result;
        }
    }
}
