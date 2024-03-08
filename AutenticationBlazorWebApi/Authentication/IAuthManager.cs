using AutenticationBlazorWebApi.Models.DTOs;
using AutenticationBlazorWebApi.Models.Responses;

namespace AutenticationBlazorWebApi.Server.Authentication
{
    public interface IAuthManager
    {
        Task<GeneralResponse> Register(UserDto userDto);

        Task<LoginResponse> Login(LoginDto loginDto);
        Task<string> CreateRefreshToken();
        Task<LoginResponse> VerifyRefreshToken(AuthResponseDto request);

    }
}
