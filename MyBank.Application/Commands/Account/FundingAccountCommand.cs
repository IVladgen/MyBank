using MediatR;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Commands.Account
{
    public class FundingAccountCommand : IRequest<BaseResponse<FundingResponse>>
    {
        public FundingDto dto { get; init; }
    }
}
