using Application.Constants;
using Application.Models.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Entities.Enum;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IRepository<Tag> tagRepository, IRepository<Role> roleRepository, UserManager<User> userManager, IMapper mapper)
        {
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CommandResult<List<UserDto>>> GetAllUsers()
        {
            // Response object 
            var responseUsers = new List<UserDto>();
            // Get all users
            var users = await _userRepository.GetAllAsync();
            foreach (var x in users)
            {

                responseUsers.Add(new UserDto
                {
                    Role = (await _userManager.GetRolesAsync(x)).FirstOrDefault() ?? String.Empty,
                    Name = x.UserName,
                    Email = x.Email,
                    Id = x.Id,
                    TagStatus = _tagRepository.GetByIdAsync(x.TagId).Result.Status.ToString() ?? String.Empty,
                });

            }
            if (!responseUsers.Any())
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
                Data = responseUsers
            };

        }

        public async Task<CommandResult<bool>> UpdateUserTag(Guid userId, TagStatus tagStatus)
        {
            // Verify if requested user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new CommandResult<bool>
                {
                    Success = false,
                    CommandError = new CommandError
                    {
                        Code = "404",
                        Description = " User was not found",
                        HttpCode = System.Net.HttpStatusCode.NotFound
                    }
                };
            }

            //Update Tag for role
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var userTag = await _tagRepository.GetByIdAsync(user.TagId);
            userTag.Status = tagStatus;
            switch (role)
            {
                case nameof(Roles.Admin):
                    userTag.StartDate = DateTime.Now;
                    userTag.ExpireDate = DateTime.Now.AddYears(Defaults.AdminLifeTime);
                    break;
                case nameof(Roles.Employee):
                    userTag.StartDate = DateTime.Now;
                    userTag.ExpireDate = DateTime.Now.AddYears(Defaults.EmployeeLifeTime);
                    break;
                case nameof(Roles.Guest):
                    userTag.StartDate = DateTime.Now;
                    userTag.ExpireDate = DateTime.Now.AddHours(Defaults.GuestLifeTime);
                    break;
                default: break;
            }

            user.Tag = userTag;
            await _userRepository.UpdateAsync(user);

            return new CommandResult<bool>
            {
                Success = true,
                Data = true
            };

        }
    }
}

