using MediatR;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Account;

namespace MyBank.Application.Queries
{
    public class GetUserAccountsQuery : IRequest<BaseResponse<List<AccountResponse>>>
    {
        
    }
}
