using Microsoft.AspNetCore.Identity;

namespace JWT_Token.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
