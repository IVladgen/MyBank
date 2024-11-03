using MediatR;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Commands.Token
{
    public class RefreshTokenCommand : IRequest<BaseResponse<RefreshResponse>>
    {
        public TokenDto dto { get; init; }
    }
}
