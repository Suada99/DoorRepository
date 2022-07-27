using Application.Models;
using Application.Models.DTOs;

namespace Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<CommandResult<bool>> RegisterUserAsync(UserRegistrationDto user);
        Task<CommandResult<AuthResult>> LogInUserAsync(UserLoginRequestDto requestUser);
        Task<CommandResult<bool>> LogOutUserAsync();
    }
}
