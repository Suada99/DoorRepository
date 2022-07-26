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
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
