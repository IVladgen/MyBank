﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Response.Account
{
    public class FundingResponse
    {
        public decimal Balance {  get; set; }
        public string Number { get; set; }
        public Guid UserId { get; set; }
    }
}
