using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyBank.Domain.Interfaces.Services;
using MyBank.Domain.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyBank.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _Issuer;
        private readonly string _Audience;
        private readonly string _Secret;
        private readonly double _AccessExpiration;
        private readonly double _RefreshExpiration;

        public TokenService(IOptions<TokenOptions> options)
        {
            _Issuer  = options.Value.Issuer;
            _Audience = options.Value.Audience;
            _Secret = options.Value.Secret;
            _RefreshExpiration = options.Value.RefreshExpiration;
            _AccessExpiration = options.Value.AccessExpiration;
        }
        
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Secret));
           
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(_Issuer, _Audience, claims, expires: DateTime.UtcNow.AddMinutes(_AccessExpiration), signingCredentials: credentials);
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        public string GenerateRefreshToken(Guid userId)
        {
            var token = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes($"{userId}:{DateTime.UtcNow.ToString("O")}"));
            var refreshToken = Convert.ToBase64String(token);
            return refreshToken;
        }

        public bool ValidateToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler().ReadToken(accessToken);

            if (tokenHandler == null) return false;
            if (tokenHandler.ValidTo < DateTime.UtcNow) return false;

            return true;
        }

    }
}
