using MediatR;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Commands.User
{
    public class CreateUserCommand : IRequest<BaseResponse>
    {
        public CreateUserDto CreateUserDto { get; init; }
    }
}
