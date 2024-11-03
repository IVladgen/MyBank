using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Response.Transfer
{
    public class TransferResponse
    {
        public TransferResponse(string numberTo, string numberFrom, decimal amount)
        {
            NumberTo = MaskNumber(numberTo);
            NumberFrom = MaskNumber(numberFrom);
            Amount = amount;
        }

        public string NumberTo { get; private set; }
        public string NumberFrom { get; private set; }
        public decimal Amount { get; private set; }

        private string MaskNumber(string number)
        {
            return new string(('*',number.Length - 4) + number.Substring(number.Length - 4));
        }
    }
}
