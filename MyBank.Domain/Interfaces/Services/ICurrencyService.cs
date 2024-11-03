using MyBank.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Interfaces.Services
{
    public interface ICurrencyService
    {
        decimal GetValueByCharCodeForTransfer(decimal currencyFrom, decimal currencyTo, decimal amount);
        Task<Currency> SetCurrencyAsync(string charCode);
        Task RefreshCurrencyDataCache();
        
    }
}
