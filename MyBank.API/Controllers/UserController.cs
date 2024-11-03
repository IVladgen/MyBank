using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBank.Application.Commands.User;
using MyBank.Application.Queries;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.User;
using MyBank.Domain.Response.Users;

namespace MyBank.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISender  _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/Create")]
        public async Task<BaseResponse> Create(CreateUserDto dto)
        {
            CreateUserCommand command = new CreateUserCommand
            { 
                CreateUserDto = dto
            };
            
            var result = await _mediator.Send(command);
            return result;
        }
        [HttpPost("/Login")]
        public async Task<BaseResponse<LoginUserResponse>> Login(LoginUserDto dto)
        {
            LoginUserCommand command = new LoginUserCommand
            {
                loginUserDto = dto
            };

            var result = await _mediator.Send(command);
            return result;
        }
        [Authorize]
        [HttpGet("/GetUserData")]
        public async Task<BaseResponse<GetUserDataResponse>>GetUserData()
        {
            GetUserDataQuery query = new GetUserDataQuery();

            var result = await _mediator.Send(query);
            return result;
        }
    }
}
