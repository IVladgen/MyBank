using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Response.Users
{
    public class GetUserDataResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NumberPhone { get; set; }
        public string Email { get; set; }
    }
}
