using Core.Entities;

namespace Core.Services
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