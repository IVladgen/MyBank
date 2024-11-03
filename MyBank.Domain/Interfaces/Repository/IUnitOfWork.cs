using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MyBank.Domain.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        Task SaveChagesAsync();
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void CommitTransaction();
        void RollbackTransaction();
    }
}
