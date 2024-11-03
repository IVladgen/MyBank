using AutoMapper;
using MyBank.Domain.Response.Transfer;
using MyBank.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBank.Domain.Entity;

namespace MyBank.Application.Mapping
{
    public class TransferMapping : Profile
    {
        public TransferMapping()
        {
            CreateMap<Transfer, GetTransfersOfAccountResponse>()
                .ForMember(dest => dest.ToUserName,
        opt => opt.MapFrom(src => src.ToAccount.User.Name))
                .ForMember(dest => dest.ToUserSurname,
        opt => opt.MapFrom(src => src.ToAccount.User.Surname))
                .ForMember(dest => dest.FromUserSurname,
        opt => opt.MapFrom(src => src.FromAccount.User.Surname))
                .ForMember(dest => dest.FromUserName,
        opt => opt.MapFrom(src => src.FromAccount.User.Name))
                .ForMember(dest => dest.Amount,
        opt => opt.MapFrom(src => src.Amount)).ReverseMap();
        }
    }
}
