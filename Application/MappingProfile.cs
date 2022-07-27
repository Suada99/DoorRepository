using Application.Models.DTOs;
using AutoMapper;
using Core.Entities;

namespace Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, User>().ReverseMap();
            CreateMap<UserDto, User>(MemberList.Source)
                .ForMember(x=>x.Email, opt=>opt.MapFrom(c=>c.Email))
                .ForMember(x=>x.UserName, opt=>opt.MapFrom(c=>c.Name))
                .ForMember(x=>x.Id, opt=>opt.MapFrom(c=>c.Id))
                .ReverseMap();
        }
    }
}
