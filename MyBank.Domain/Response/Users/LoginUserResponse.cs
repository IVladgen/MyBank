using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Response.Users
{
    public class LoginUserResponse
    {
        public Guid userId { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime validTo { get; set; }
    }
}
