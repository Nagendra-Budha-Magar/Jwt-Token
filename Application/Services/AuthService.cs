using JWT_Token.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        public async Task<AppResponseDto> Login (LoginDto dto)
        {
            // Checking if the user exist
            var user = await _userManager.FindByNameAsync(dto.Username);
            if(user == null)
            {
                return new AppResponseDto
                {
                    Success = false,
                    Errors = new List<string> { "Invalid Username or Password" }
                };
            }

            // Checking password
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
            {
                return new AppResponseDto
                {
                    Success = false,
                    Errors = new List<string> { "Invalid Username or Password"}
                };
            }

            var token = await CreateToken(user);

            return new AppResponseDto
            {
                Success = true,
                Token = token
            };
        }

        private async Task<string> CreateToken(IdentityUser user)
        {
            // This is the information about a subject.(Subject means user or admin)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),  // What is subject id
                new Claim(ClaimTypes.Name, user.UserName)       // What is subject name
            };
            // Adding role of a Subject
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Getting Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            // Need for verifying if the token are correct using key + hash algorithm. Used for making 'Signature'
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            // Blueprint of token
            var TokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds, // Tts a signuture
                Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                Audience = _configuration.GetSection("Jwt:Audience").Value
            };
            // Actually creating and converting token into string
            var TokenHandler = new JwtSecurityTokenHandler();
            var Token = TokenHandler.CreateToken(TokenDecriptor);
            return TokenHandler.WriteToken(Token);
        }
    }
}
