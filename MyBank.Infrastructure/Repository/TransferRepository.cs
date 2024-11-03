using Microsoft.EntityFrameworkCore;
using MyBank.Domain.DTO;
using MyBank.Domain.Entity;
using MyBank.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Infrastructure.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly AppDbContext _context;

        public TransferRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transfer> AddAsync(Transfer transfer)
        {
            await _context.Transfers.AddAsync(transfer);
            return transfer;
        }

        public async Task<List<Transfer>> GetTransfersByAccountIdAsync(Guid accountId)
        {
           return await _context.Transfers
            .Include(t => t.FromAccount)
                .ThenInclude(ba => ba.User)
            .Include(t => t.ToAccount)
                .ThenInclude(ba => ba.User) 
            .Where(t => t.AccountFromId == accountId || t.AccountToId == accountId)
            .ToListAsync();
        }
    }
}
