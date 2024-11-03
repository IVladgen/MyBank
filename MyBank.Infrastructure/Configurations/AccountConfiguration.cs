using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBank.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Infrastructure.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Number).IsRequired();

            builder.HasMany(b => b.TransfersFrom)
            .WithOne(t => t.FromAccount)
            .HasForeignKey(t => t.AccountFromId);

            builder.HasMany(b => b.TransfersTo)
            .WithOne(t => t.ToAccount)
            .HasForeignKey(t => t.AccountToId);

            builder.OwnsOne(a => a.Currency, CurrencyBuilder =>
            {
                CurrencyBuilder.Property(c => c.Value).IsRequired();
                CurrencyBuilder.Property(c => c.CharCode).IsRequired();
            });
        }
    }
}
