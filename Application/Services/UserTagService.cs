using Core.Entities;
using Core.Entities.Enum;
using Core.Repositories;

namespace Application.Services
{
    public class UserTagService
    {
        private readonly IRepository<UserTag> _userTagRepository;

        public UserTagService(IRepository<UserTag> userTagRepository)
        {
            _userTagRepository = userTagRepository;
        }

        public async Task<List<UserTag>> GetAllTagStatusesByUserAsync(Guid userId, TagStatus? tagStatus = null,
            DateTime? startDate = null, DateTime? expireDate = null)
        {
            var query = await _userTagRepository.GetWhereAsync(x => x.UserId == userId);
            if (tagStatus.HasValue)
            {
                query = query.Where(x => x.Status == tagStatus.Value);
            }
            if (startDate.HasValue && expireDate.HasValue)
            {
                query = query.Where(x => x.StartDate >= startDate.Value && x.ExpireDate <= expireDate.Value);
            }
            return query.ToList();
        }


        public async Task InsertTagStatusAsync(UserTag userTag)
        {
            await _userTagRepository.AddAsync(userTag);
        }
    }
}
