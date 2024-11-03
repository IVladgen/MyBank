using MediatR;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Queries
{
    public class GetAccountsByNumberPhoneQuery : IRequest<BaseResponse<List<GetAccountsByNumberPhoneResponse>>>
    {
        public GetAccountsByNumberPhoneDto dto { get; set; }
    }
}
