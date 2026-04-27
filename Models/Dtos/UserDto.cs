using System.ComponentModel.DataAnnotations;

namespace JWT_Token.Models.Dtos
{
    public class RegisterDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        [MinLength(7)]
        public required string Password { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        [MinLength(7)]
        public required string Password { get; set; }
    }
}
