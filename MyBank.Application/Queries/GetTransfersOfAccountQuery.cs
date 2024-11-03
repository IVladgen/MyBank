using MediatR;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Queries
{
    public class GetTransfersOfAccountQuery : IRequest<BaseResponse<List<GetTransfersOfAccountResponse>>>
    {
        public GetTransfersOfAccountDto dto { get; init; }
    }
}
