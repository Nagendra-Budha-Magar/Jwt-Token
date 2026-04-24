using JWT_Token.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace JWT_Token.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AppResponseDto> Register (RegisterDto dto, string role)
        {

            // Mapping Register Dto first
            var user = new IdentityUser
            {
                UserName = dto.Username
            };

            // Validate inputs
            if (string.IsNullOrWhiteSpace(role)){
                return new AppResponseDto
                {
                    Success = false,
                    Errors = new List<string> { "Role cannot  be empty"}
                };
            }

            // Registering new User
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return new AppResponseDto
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            // Assigning Role
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                return new AppResponseDto
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            return new AppResponseDto
            {
                Success = true
            };

        }

    }
}
