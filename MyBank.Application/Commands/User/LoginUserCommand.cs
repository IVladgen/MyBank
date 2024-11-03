using MediatR;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Commands.User
{
    public class LoginUserCommand : IRequest<BaseResponse<LoginUserResponse>>
    {
        public LoginUserDto loginUserDto { get; init; }
    }
}
