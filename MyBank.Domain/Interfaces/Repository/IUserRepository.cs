using MyBank.Domain.DTO;
using MyBank.Domain.Entity;
using MyBank.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Interfaces.Repository
{
    public interface IUserRepository 
    {
        Task<User> AddAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByNumberPhoneAsync(string numberPhone);
        Task<bool> UserExistsByEmail(string email);
        Task<bool> UserExistsByNumberPhone(string numberPhone);
        User GetUserByToken(string token);
    }
}
