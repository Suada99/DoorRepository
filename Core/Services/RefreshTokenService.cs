using Core.Entities;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Core.Services
{
    public partial class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        public RefreshTokenService(IRepository<RefreshToken> refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public virtual async Task<RefreshToken> GetRefreshTokenByIdAsync(Guid guid)
        {
            return await _refreshTokenRepository.GetById(guid);
        }

        public virtual async Task<RefreshToken> GetRefreshTokenByRefreshTokeAsync(string refreshToken)
        {
            return (await _refreshTokenRepository.GetWhere(x => x.Token == refreshToken)).FirstOrDefault();
        }

        public virtual async Task InsertRefreshTokenAsync(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException("RefreshToken");

            await _refreshTokenRepository.Add(refreshToken);
        }

        public virtual async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException("RefreshToken");

            await _refreshTokenRepository.Update(refreshToken);
        }

        public virtual async Task DeleteRefreshTokenAsync(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException("RefreshToken");

            await _refreshTokenRepository.Remove(refreshToken);
        }
    }
}
