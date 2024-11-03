using MediatR;
using MyBank.Application.Commands.Transfers;
using MyBank.Application.Recourses;
using MyBank.Domain.Entity;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Transfer;
using MyBank.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Handlers.Commands.Transfers
{
    public class MoneyTransferHandler : IRequestHandler<MoneyTransferCommand, BaseResponse<TransferResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoneyTransferHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ITransferRepository transferRepository)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _transferRepository = transferRepository;
        }

        public async Task<BaseResponse<TransferResponse>> Handle(MoneyTransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var accountFrom = await _accountRepository.GetAccountByIdAsync(request.dto.SenderId);
                var accountTo = await _accountRepository.GetAccountByNumberAsync(request.dto.NumberReceiver);
                var response = new BaseResponse<TransferResponse>();

                if (accountFrom == null || accountTo == null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.AddError(ErrorMessages.AccountNotFound);
                    return response;
                }

                accountFrom.Debit(request.dto.Amount);
                accountTo.Credit(request.dto.Amount);
                var transfer = new Transfer(accountFrom.Id, accountTo.Id, request.dto.Amount);

                await _transferRepository.AddAsync(transfer);


                var TransferResponse = new TransferResponse(accountFrom.Number, accountTo.Number, transfer.Amount);

                
                response.AddData(TransferResponse);

                return response;
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
