using Application.Models.DTOs;
using Core.Entities;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<CommandResult<List<UserDto>>> GetAllUsers();
    }
}