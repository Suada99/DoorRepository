using Application.Services.Interfaces;
using Core.Entities;
using Core.Repositories;

namespace Application.Services
{
    public partial class JWTTokenService : IJWTTokenService
    {
        private readonly IRepository<JWTToken> _jwtTokenRepository;

        public JWTTokenService(IRepository<JWTToken> jwtTokenRepository)
        {
            _jwtTokenRepository = jwtTokenRepository;
        }

        public virtual async Task<JWTToken> GeJWTTokenByIdAsync(Guid guid)
        {
            return await _jwtTokenRepository.GetByIdAsync(guid);
        }

        public virtual async Task<JWTToken> GetJWTTokenByTokenAsync(string jwtToken)
        {
            return (await _jwtTokenRepository.GetWhereAsync(x => x.Token == jwtToken)).OrderByDescending(x=>x.AddedDate).FirstOrDefault();
        }


        public virtual async Task InsertJWTTokenAsync(JWTToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException("JWTToken");

            await _jwtTokenRepository.AddAsync(refreshToken);
        }

        public virtual async Task UpdateJWTTokenAsync(JWTToken jwtToken)
        {
            if (jwtToken == null)
                throw new ArgumentNullException("JWTToken");

            await _jwtTokenRepository.UpdateAsync(jwtToken);
        }

        public virtual async Task DeleteJWTTokenAsync(JWTToken jwtToken)
        {
            if (jwtToken == null)
                throw new ArgumentNullException("JWTToken");

            await _jwtTokenRepository.DeleteAsync(jwtToken);
        }
    }
}
