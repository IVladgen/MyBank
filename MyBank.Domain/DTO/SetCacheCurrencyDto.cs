﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.DTO
{
    public record SetCacheCurrencyDto(string CharCode, decimal Value);
}
