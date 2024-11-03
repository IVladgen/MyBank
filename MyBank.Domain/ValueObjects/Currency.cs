using System.ComponentModel.DataAnnotations.Schema;

namespace MyBank.Domain.ValueObjects
{

    public class Currency
    {

        private Currency() { }
        private Currency(string charCode, decimal value)
        {
            CharCode = charCode;
            Value = value;
        }


        public string CharCode { get; private set; }
        public decimal Value { get; private set; }

        public static Currency Create(string charCode, decimal value)
        {
            return new Currency(charCode, value);
        }
    }
}
