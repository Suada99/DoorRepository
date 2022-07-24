using Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class JWTToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Deactivated { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
