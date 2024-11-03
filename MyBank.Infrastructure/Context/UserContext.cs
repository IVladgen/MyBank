using Microsoft.AspNetCore.Http;
using MyBank.Domain.Interfaces.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Infrastructure.Context
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid GetUserIdWithClaims()
        {
            var claims = _contextAccessor.HttpContext.User;
            var userIdStr = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdStr, out var userId))
            {
                return userId;
            }

            throw new InvalidOperationException("Неверный формат ID");
        }
    }
}
