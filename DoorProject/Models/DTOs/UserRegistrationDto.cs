using System.ComponentModel.DataAnnotations;

namespace DoorProject.Models.DTOs
{
    public class UserRegistrationDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Please try a stronger password")]
        public string Password { get; set; }
    }
}
