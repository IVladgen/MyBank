using MyBank.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Entity
{
    public class Transfer : EntityId<Guid>
    {
        public Transfer(Guid accountFromId, Guid accountToId, decimal amount)
        {
            AccountFromId = accountFromId;
            AccountToId = accountToId;
            Amount = amount;
        }

        public Guid AccountFromId { get; set; }
        public Guid AccountToId { get; set; }

        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }

        public decimal Amount { get; private set; }
    }
}
