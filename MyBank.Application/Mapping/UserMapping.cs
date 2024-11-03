using AutoMapper;
using MyBank.Domain.DTO;
using MyBank.Domain.Entity;
using MyBank.Domain.Response.User;
using MyBank.Domain.Response.Users;

namespace MyBank.Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, CreateUserResponse>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, GetUserDataResponse>().ReverseMap();
        }
    }
}
