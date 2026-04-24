using System.ComponentModel.DataAnnotations;

namespace JWT_Token.Models.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(7)]
        public string Password { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(7)]
        public string Password { get; set; }
    }
}
