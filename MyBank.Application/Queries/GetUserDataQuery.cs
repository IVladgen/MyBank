using MediatR;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Queries
{
    public class GetUserDataQuery : IRequest<BaseResponse<GetUserDataResponse>>
    {
    }
}
