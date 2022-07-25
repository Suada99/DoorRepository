using Core.Entities;

namespace Core.Services.Interfaces
{
    public interface IWorkContext
    {
        bool IsAdmin { get; set; }

        Task DeactivateCurrentTokenAsync();
        Task DeactivateTokenAsync(string token);
        Task<User> GetCurrentUserAsync();
        Task<bool> IsActiveTokenAsync(string token);
        Task<bool> IsCurrentActiveToken();
    }
}