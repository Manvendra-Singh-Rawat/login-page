using System.ComponentModel.DataAnnotations;

namespace LoginPage.Entity
{
    public class User
    {
        [Required]
        public int Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
