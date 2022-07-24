using AutoMapper;
using Core.Entities;
using DoorProject.Models.DTOs;

namespace Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, User>().ReverseMap();
        }
    }
}
