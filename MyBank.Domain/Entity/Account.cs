using MyBank.Domain.Primitives;
using MyBank.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Entity
{
    public class Account : EntityId<Guid>
    {
        public Account(Currency currency)
        {
            Id = Guid.NewGuid();
            Balance = 0;
            Number = GenerateNumber();
            Currency = currency;
        }

        public Account(Guid userId, Currency currency) : this(currency)
        {
            UserId = userId;
        }

        public string Number { get; private set; }
        public decimal Balance { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public List<Transfer> TransfersFrom { get; private set; }
        public List<Transfer> TransfersTo { get; private set; }
        public Currency Currency { get; private set; }

        public Currency SetCurrency(Currency currency)
        {
            return Currency;
        }

        public string GenerateNumber()
        {
            Random random = new Random();
            return new string(Enumerable.Range(0, 20)
                                     .Select(i => (char)('0' + random.Next(0, 9)))
                                     .ToArray());
        }

        public decimal TopUp(decimal amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), $"сумма пополнения должна быть > 0");
            Balance =+ amount;
            return Balance;
        }

        public decimal Debit(decimal amount) 
        {
            if (Balance < amount) throw new ArgumentOutOfRangeException(nameof(amount), $"сумма перевода не может превышать баланс счета");
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), $"сумма перевода должна быть > 0");
            Balance -= amount;
            return Balance;
        }

        public decimal Credit(decimal amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), $"сумма пополнения должна быть > 0");
            Balance += amount;
            return Balance;
        }

        public Account()
        {

        }
    }
}
