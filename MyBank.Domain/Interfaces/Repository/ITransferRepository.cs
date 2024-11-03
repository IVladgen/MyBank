using MyBank.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Interfaces.Repository
{
    public interface ITransferRepository
    {
        Task<Transfer> AddAsync(Transfer transfer);
        Task<List<Transfer>> GetTransfersByAccountIdAsync(Guid accountId);
    }
}
