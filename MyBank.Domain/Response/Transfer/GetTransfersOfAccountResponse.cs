using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Response.Transfer
{
    public class GetTransfersOfAccountResponse
    {
        public decimal Amount { get; set; }
        public string FromUserName { get; set; }
        public string FromUserSurname { get; set; }
        public string ToUserName { get; set; }
        public string ToUserSurname { get; set; }
        public bool isSender { get; set; }
    }
}
