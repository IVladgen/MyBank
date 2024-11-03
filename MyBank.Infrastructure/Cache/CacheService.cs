using JsonNet.ContractResolvers;
using Microsoft.Extensions.Caching.Distributed;
using MyBank.Domain.Interfaces.Cache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Infrastructure.Cache
{
    public class CacheService<T> : ICacheService<T> where T : class
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task DeleteDataAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task<T> GetDataAsync(string key)
        {
            var result = await _cache.GetStringAsync(key);
            
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new PrivateSetterContractResolver()
            };

            if (result != null)
            {
                T a = JsonConvert.DeserializeObject<T>(result);
                return a;
            }

            return null;
        }

        public async Task SetDataAsync(string key, T data, TimeSpan expirationTime)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new PrivateSetterContractResolver()
            };

            var serializedData = JsonConvert.SerializeObject(data);

            var bytes = Encoding.UTF8.GetBytes(serializedData);

            await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            });
        }

    }
}
