using System.ComponentModel.DataAnnotations;

namespace Application.Models.DTOs
{
    public class UserRegistrationDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", ErrorMessage = $"Invalid Password! " + $"Password must be at least 8 characters long " + $" and contain at least one lowercase, one uppercase, one number and one special character.")] 
        public string Password { get; set; }
    }
}
