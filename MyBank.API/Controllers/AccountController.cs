using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBank.Application.Commands.Account;
using MyBank.Application.Commands.Transfers;
using MyBank.Application.Queries;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Account;
using MyBank.Domain.Response.Transfer;

namespace MyBank.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ISender _mediator;

        public AccountController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Accounts")]
        public async Task<BaseResponse<List<AccountResponse>>> GetUserAccounts()
        {
            var query = new GetUserAccountsQuery();

            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPost("/CreateAccount")]
        public async Task<BaseResponse<CreateAccountResponse>> CreateAccount(CreateAccountDto dto)
        {
            var command = new CreateAccountCommand()
            { dto = dto};

            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost("/FundingAccount")]
        public async Task<BaseResponse<FundingResponse>> FundingAccount(FundingDto Dto)
        {
            var command = new FundingAccountCommand { dto = Dto };

            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost("/TransferAmount")]
        public async Task<BaseResponse<TransferResponse>> TransferAmount(MoneyTransferDto Dto)
        {
            var command = new MoneyTransferCommand { dto = Dto };

            var result = await _mediator.Send(command);
            return result;
        }

        [HttpGet("/GetAccountsByNumberPhone")]
        public async Task<BaseResponse<List<GetAccountsByNumberPhoneResponse>>> TransferByNumberPhone([FromQuery]GetAccountsByNumberPhoneDto dto)
        {
            var query = new GetAccountsByNumberPhoneQuery()
            { dto = dto };

            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("/GetTransfersOfAccount")]
        public async Task<BaseResponse<List<GetTransfersOfAccountResponse>>> GetTransfersOfAccount([FromQuery]GetTransfersOfAccountDto Dto)
        {
            var query = new GetTransfersOfAccountQuery()
            { dto = Dto };

            var result = await _mediator.Send(query);
            return result;
        }
    }
}
