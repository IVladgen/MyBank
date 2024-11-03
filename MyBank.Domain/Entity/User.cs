using MyBank.Domain.Enums;
using MyBank.Domain.Primitives;
using MyBank.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Entity
{
    public class User : EntityId<Guid>
    {
        public User(string name, string surname, string numberPhone, string email,string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            NumberPhone = numberPhone;
            Email = email;
            Password = password;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string NumberPhone { get; private set; }
        public string Email { get; private set; }
        public bool VerifyEmail { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }
        public List<Account> Accounts { get; set; }
        public Token? Token { get; private set; }

        public void AddUserToken(string refreshToken, DateTime expireTime)
        {
            if (refreshToken is null) return;
            Token = new Token(refreshToken, expireTime);
        }
        public void SetRole(UserRole userRole) => Role = userRole.ToString();

        public static User Create(string name, string surname, string numberPhone, string email, string password)
        {
            var user = new User(name, surname, numberPhone, email, password);
            user.SetRole(UserRole.User);
            return user;
        }

        public void Verify()
        {
            VerifyEmail = true;
        }
    }
}
