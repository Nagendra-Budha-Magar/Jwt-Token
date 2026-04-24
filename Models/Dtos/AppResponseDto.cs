using Microsoft.AspNetCore.Identity;

namespace JWT_Token.Models.Dtos
{
    // It is used to return in Register and login like other Dtos
    public class AppResponseDto
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
