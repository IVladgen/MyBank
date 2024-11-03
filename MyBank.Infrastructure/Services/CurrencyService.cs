using MyBank.Domain.DTO;
using MyBank.Domain.Enums;
using MyBank.Domain.Interfaces.Cache;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Interfaces.Services;
using MyBank.Domain.ValueObjects;
using System.Text;
using System.Xml.Linq;

namespace MyBank.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IAccountRepository _account;
        private static readonly string url = "https://www.cbr.ru/scripts/XML_daily.asp";
        private readonly ICacheService<SetCacheCurrencyDto> _cache;

        public CurrencyService(HttpClient httpClient, IAccountRepository account, ICacheService<SetCacheCurrencyDto> cache)
        {
            _httpClient = httpClient;
            _account = account;
            _cache = cache;
        }

        public decimal GetValueByCharCodeForTransfer(decimal currencyFrom, decimal currencyTo, decimal amount)
        {
            decimal coefficientOfTransfer = currencyFrom / currencyTo;
            decimal transfer = coefficientOfTransfer * amount;
            return transfer;
        }

        public async Task<Currency> SetCurrencyAsync(string charCode)
        {
            try
            {
                if (charCode == "RUB")
                {
                    return Currency.Create(charCode, 1);
                }
                var data = await _cache.GetDataAsync(charCode.ToUpper());

                if (data != null)
                {
                    return Currency.Create(data.CharCode, data.Value);
                }
                if (!Enum.TryParse(typeof(CurrencyCode), charCode.ToUpper(), out var currencyCode))
                {
                    throw new ArgumentException($"'{charCode}' is not a valid currency code.");
                }

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{response.StatusCode}");
                }

                var bytes = await response.Content.ReadAsByteArrayAsync();
                var encoding = Encoding.UTF8;
                var result = encoding.GetString(bytes);

                XDocument xmlDocument = XDocument.Parse(result);

                var XCurrencys = xmlDocument.Descendants("Valute");

                foreach ( var XCurrency in XCurrencys )
                {
                    var charCodeCache = (string)XCurrency.Element("CharCode");
                    var valueCache = decimal.Parse((string)XCurrency.Element("Value"));
                    string cacheKey = $"{charCodeCache.ToUpper()}";
                    await _cache.SetDataAsync(cacheKey, new SetCacheCurrencyDto(charCodeCache, valueCache), TimeSpan.FromHours(3));
                }

                var dto = await _cache.GetDataAsync(charCode.ToUpper());

                return Currency.Create(dto.CharCode, dto.Value);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RefreshCurrencyDataCache()
        {
            var currencyResponse = await _httpClient.GetAsync(url);
            var bytes = await currencyResponse.Content.ReadAsByteArrayAsync();
            var encoding = Encoding.UTF8.GetString(bytes);

            XDocument xmlCurrency = XDocument.Parse(encoding);
            var result = xmlCurrency.Descendants("Valute");

            foreach (var XValue in result)
            {
                var charCode = (string)XValue.Element("CharCode");
                var value = decimal.Parse((string)XValue.Element("Value"));
                string cacheKey = $"{charCode.ToUpper()}";
                await _cache.SetDataAsync(cacheKey, new SetCacheCurrencyDto(charCode, value), TimeSpan.FromHours(3));
            }
        }
    }
}
