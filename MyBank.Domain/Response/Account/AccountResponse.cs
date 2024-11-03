using MyBank.Domain.ValueObjects;

namespace MyBank.Domain.Response.Account
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
        public List<Entity.Transfer> Transfers { get; set; }
    }
}
