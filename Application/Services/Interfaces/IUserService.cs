using Core.Entities;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task DeleteUserAsync(User User);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(Guid guid);
        Task InsertUserAsync(User User);
        Task UpdateUserAsync(User User);
    }
}