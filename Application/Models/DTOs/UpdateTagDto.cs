using Core.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.DTOs
{
    public class UpdateTagDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public TagStatus TagStatus { get; set; }
    }
}
