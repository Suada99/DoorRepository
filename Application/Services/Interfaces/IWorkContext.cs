using Core.Entities;

namespace Application.Services.Interfacess
{
    public interface IWorkContext
    {
        Task DeactivateCurrentTokenAsync();
        Task DeactivateTokenAsync(string token);
        Task<User> GetCurrentUserAsync();
        Task<bool> IsActiveTokenAsync(string token);
        Task<bool> IsCurrentActiveToken();
    }
}