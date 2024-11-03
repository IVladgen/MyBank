using Microsoft.EntityFrameworkCore;
using MyBank.Domain.DTO;
using MyBank.Domain.Entity;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
        
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<User> GetUserByNumberPhoneAsync(string numberPhone)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.NumberPhone == numberPhone);
            return user;
        }

        public async Task<bool> UserExistsByEmail(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> UserExistsByNumberPhone(string numberPhone)
        {
            return await _context.Users.AnyAsync(x => x.NumberPhone == numberPhone);
        }

        public User GetUserByToken(string token)
        {
            var user = _context.Users.FirstOrDefault(x => x.Token.RefreshToken == token);
            return user;
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

    }
}
