using AutoMapper;
using MyBank.Domain.Entity;
using MyBank.Domain.Response.Account;

namespace MyBank.Application.Mapping
{
    public class AccountMapping : Profile
    {
        public AccountMapping()
        {
            CreateMap<Account, AccountResponse>().ReverseMap();
            CreateMap<Account, CreateAccountResponse>().ReverseMap();
            CreateMap<Account, FundingResponse>().ReverseMap();
            CreateMap<Account, GetAccountsByNumberPhoneResponse>().ReverseMap();
        }
    }
}
