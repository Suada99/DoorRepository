using Core.Entities;

namespace Core.Services.Interfaces
{
    public interface IJWTTokenService
    {
        Task DeleteJWTTokenAsync(JWTToken refreshToken);
        Task<JWTToken> GeJWTTokenByIdAsync(Guid guid);
        Task<JWTToken> GetJWTTokenByTokenAsync(string refreshToken);
        Task InsertJWTTokenAsync(JWTToken refreshToken);
        Task UpdateJWTTokenAsync(JWTToken refreshToken);
    }
}