using Microsoft.EntityFrameworkCore;
using MyBank.Domain.Entity;
using MyBank.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> Add(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account> GetUsersAccountByNumberAsync(Guid userId, string number)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == userId && x.Number == number);
            return account;
        }

        public async Task<List<Account>> GetUserAccountsByIdNoTracingAsync(Guid userId)
        {
            var accounts = await _context.Accounts
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .AsQueryable()
                .ToListAsync();
            
                return accounts;
        }

        public async Task<Account> GetAccountByIdAsync(Guid accountId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            return account;
        }

        public async Task<Account> GetAccountByNumberAsync(string number)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Number == number);
            return account;
        }

        public async Task<List<Account>> GetAccountsByNumberPhone(string numberPhone)
        {
            var accounts = await _context.Accounts.Where(x => x.User.NumberPhone == numberPhone).ToListAsync();
            return accounts;
        }
    }
}
