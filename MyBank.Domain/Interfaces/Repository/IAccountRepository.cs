using MyBank.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Interfaces.Repository
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetUserAccountsByIdNoTracingAsync(Guid userId);
        Task<List<Account>> GetAccountsByNumberPhone(string numberPhone);
        Task<Account> Add(Account account);
        Task<Account> GetUsersAccountByNumberAsync(Guid userId, string number);
        Task<Account> GetAccountByIdAsync(Guid accountId);
        Task<Account> GetAccountByNumberAsync(string number);
    }
}
