using MediatR;
using MyBank.Domain.DTO;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Commands.Transfers
{
    public class MoneyTransferCommand : IRequest<BaseResponse<TransferResponse>>
    {
        public MoneyTransferDto dto { get; init; }
    }
}
