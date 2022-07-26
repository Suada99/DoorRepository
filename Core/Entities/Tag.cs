using Core.Entities.Enum;

namespace Core.Entities
{
    public class Tag : BaseEntity
    {
        public TagStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
