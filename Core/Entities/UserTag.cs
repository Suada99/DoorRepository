using Core.Entities.Enum;

namespace Core.Entities
{
    public class UserTag : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public TagStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool Deactivated { get; set; }
    }
}
