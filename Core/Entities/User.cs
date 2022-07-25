using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public int Age { get; set; }
        public bool InOffice { get; set; }
    }
}
