using Core.Entities.Enum;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public bool InOffice { get; set; }
        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
