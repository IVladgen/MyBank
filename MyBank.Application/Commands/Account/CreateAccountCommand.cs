using MediatR;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Account;

namespace MyBank.Application.Commands.Account
{
    public class CreateAccountCommand : IRequest<BaseResponse<CreateAccountResponse>>
    {
        public CreateAccountDto dto { get; init; }
    }
}
