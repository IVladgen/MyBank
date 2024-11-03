using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.ValueObjects
{
    public class Token
    {
        public string RefreshToken { get; private set; }
        public DateTime RefreshTokenExpiration { get; set; }

        public Token(string refreshToken, DateTime refreshTokenExpiration)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
        }
    }
}
