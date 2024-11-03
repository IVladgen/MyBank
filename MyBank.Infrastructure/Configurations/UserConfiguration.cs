using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBank.Domain.Entity;
using MyBank.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id); // Установка первичного ключа

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256); // Максимальная длина email

            builder.Property(e => e.Password)
                .IsRequired(); // Обязательное поле

            builder.Property(e => e.Role)
                .HasMaxLength(50); // Максимальная длина поля

            builder.Property(e => e.VerifyEmail)
                .IsRequired(); // Обязательное поле (по желанию)

            builder.OwnsOne(u => u.Token, Tokenbuilder =>
            {
                 Tokenbuilder.Property(t => t.RefreshToken).IsRequired(false);
                 
            });
            builder.Navigation(u => u.Token).IsRequired(false);

        }
    }
}
