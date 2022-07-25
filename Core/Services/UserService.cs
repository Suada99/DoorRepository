using Core.Entities;
using Core.Services.Interfaces;
using Shared.Interfaces;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public virtual async Task<User> GetUserByIdAsync(Guid guid)
        {
            return await _userRepository.GetById(guid);
        }

        public virtual async Task<User> GetUserByEmailAsync(string email)
        {
            return (await _userRepository.GetWhere(x => x.Email == email)).FirstOrDefault();
        }

        public virtual async Task InsertUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User");

            await _userRepository.Add(user);
        }

        public virtual async Task UpdateUserAsync(User User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            await _userRepository.Update(User);
        }

        public virtual async Task DeleteUserAsync(User User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            await _userRepository.Remove(User);
        }
    }
}
