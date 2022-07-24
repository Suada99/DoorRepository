using Core.Entities;

namespace Core.Services
{
    public interface IRefreshTokenService
    {
        Task DeleteRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByIdAsync(Guid guid);
        Task<RefreshToken> GetRefreshTokenByRefreshTokeAsync(string refreshToken);
        Task InsertRefreshTokenAsync(RefreshToken refreshToken);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}