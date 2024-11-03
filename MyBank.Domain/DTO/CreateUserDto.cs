using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.DTO
{
    public record CreateUserDto(string Email,string NumberPhone, string Name, string Surname, string Password);
}
