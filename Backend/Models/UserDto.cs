using System.ComponentModel.DataAnnotations;

namespace LoginPage.Models
{
    public class UserDto
    {
        [Required]
        [MaxLength(64, ErrorMessage = "Max length cannot exceed 5 characters")]
        [RegularExpression("^[a-zA-Z0-9_.]+$", ErrorMessage = "Only Characters and Numbers are allowed.")]
        public required string username { get; set; }

        [Required]
        public required string password { get; set; }
    }
}
