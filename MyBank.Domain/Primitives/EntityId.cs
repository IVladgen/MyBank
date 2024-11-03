using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Primitives
{
    public abstract class EntityId<TEntityId>
    {
        public TEntityId Id { get; set; }
    }
}
