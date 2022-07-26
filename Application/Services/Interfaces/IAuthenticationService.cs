using Application.Models;
using Application.Models.DTOs;
using Core.Entities;

namespace Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<CommandResult<User>> RegisterUserAsync(UserRegistrationDto user);
        Task<CommandResult<AuthResult>> LogInUserAsync(UserLoginRequestDto requestUser);
        Task<CommandResult<bool>> LogOutUserAsync();
    }
}
