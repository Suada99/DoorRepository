using Application.Models.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Repositories;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public virtual async Task<CommandResult<List<UserDto>>> GetAllUsers()
        {
            // Get all users
            var users = await _userRepository.GetAllAsync();
            // Map to DTO
            var mappedUsers = _mapper.Map<List<UserDto>>(users);
            if (!mappedUsers.Any())
            {
                return new CommandResult<List<UserDto>>
                {
                    Success = false,
                    TotalDataCount = 0,
                };
            }
            // Response data
            return new CommandResult<List<UserDto>>
            {
                Success = true,
                TotalDataCount = users.Count,
                Data = mappedUsers
            };

        }
    }
}
