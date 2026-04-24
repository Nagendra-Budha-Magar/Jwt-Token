using JWT_Token.Models.Dtos;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace JWT_Token.Application.Services
{
    public interface IAuthService
    {
        Task<AppResponseDto> Register(RegisterDto dto, String role);
        Task<bool> UserExist(string username);
        Task<AppResponseDto> Login(LoginDto dto);
    }
}
