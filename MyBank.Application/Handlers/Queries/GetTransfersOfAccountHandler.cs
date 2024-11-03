using AutoMapper;
using MediatR;
using MyBank.Application.Queries;
using MyBank.Domain.DTO;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Handlers.Queries
{
    public class GetTransfersOfAccountHandler : IRequestHandler<GetTransfersOfAccountQuery, BaseResponse<List<GetTransfersOfAccountResponse>>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IMapper _mapper;

        public GetTransfersOfAccountHandler(IAccountRepository accountRepository, ITransferRepository transferRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transferRepository = transferRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<GetTransfersOfAccountResponse>>> Handle(GetTransfersOfAccountQuery request, CancellationToken cancellationToken)
        {
            var transfers = await _transferRepository.GetTransfersByAccountIdAsync(request.dto.Id);
            var response = new BaseResponse<List<GetTransfersOfAccountResponse>>();
            var result = transfers.Select(t => new GetTransfersOfAccountResponse
            {
                Amount = t.Amount,
                FromUserName = t.FromAccount.User.Name,
                FromUserSurname = t.FromAccount.User.Surname,
                ToUserName = t.ToAccount.User.Name,
                ToUserSurname = t.ToAccount.User.Surname,
                isSender = t.AccountFromId == request.dto.Id
            }).ToList();

            response.AddData(result);
            return response;
        }
    }
}
