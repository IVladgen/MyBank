using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Interfaces.Cache
{
    public interface ICacheService<T> where T : class
    {
        Task DeleteDataAsync(string key);
        Task<T> GetDataAsync(string key);
        Task SetDataAsync(string key, T data, TimeSpan expirationTime);
    }
}
