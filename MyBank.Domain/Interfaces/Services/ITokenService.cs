using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateRefreshToken(Guid userId);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        bool ValidateToken(string accessToken);
    }
}
