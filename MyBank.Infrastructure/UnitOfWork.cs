using Microsoft.EntityFrameworkCore;
using MyBank.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MyBank.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transaction = new CommittableTransaction
            (new TransactionOptions { IsolationLevel = isolationLevel });

            _context.Database.OpenConnection();
            _context.Database.EnlistTransaction(transaction);
        }

        public void CommitTransaction()
        {
            var transaction = (CommittableTransaction)_context.Database.GetEnlistedTransaction();
            transaction.Commit();
            transaction.Dispose();
        }

        public void RollbackTransaction()
        {
            var transaction = _context.Database.GetEnlistedTransaction();
            transaction.Rollback();
            transaction.Dispose();
        }

        public async Task SaveChagesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
