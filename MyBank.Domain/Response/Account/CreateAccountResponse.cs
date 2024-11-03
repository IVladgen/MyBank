using MyBank.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Response.Account
{
    public class CreateAccountResponse
    {
        public Guid Id {  get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency {  get; set; }
    }
}
