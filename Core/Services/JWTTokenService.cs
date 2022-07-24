using Core.Entities;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Core.Services
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
            return await _jwtTokenRepository.GetById(guid);
        }

        public virtual async Task<JWTToken> GetJWTTokenByTokenAsync(string jwtToken)
        {
            return (await _jwtTokenRepository.GetWhere(x => x.Token == jwtToken)).OrderByDescending(x=>x.AddedDate).FirstOrDefault();
        }


        public virtual async Task InsertJWTTokenAsync(JWTToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException("JWTToken");

            await _jwtTokenRepository.Add(refreshToken);
        }

        public virtual async Task UpdateJWTTokenAsync(JWTToken jwtToken)
        {
            if (jwtToken == null)
                throw new ArgumentNullException("JWTToken");

            await _jwtTokenRepository.Update(jwtToken);
        }

        public virtual async Task DeleteJWTTokenAsync(JWTToken jwtToken)
        {
            if (jwtToken == null)
                throw new ArgumentNullException("JWTToken");

            await _jwtTokenRepository.Remove(jwtToken);
        }
    }
}
