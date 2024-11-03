using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Helpers
{
    public static class CacheKeys
    {
        public static string GetUsersAccountByIdCacheKey(int userId) => $"user:{userId}:accounts";
    }
}
