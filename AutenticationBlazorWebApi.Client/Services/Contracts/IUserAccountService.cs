using AutenticationBlazorWebApi.Models.DTOs;
using AutenticationBlazorWebApi.Models.Responses;

namespace AutenticationBlazorWebApi.Client.Services.Contracts
{
    public interface IUserAccountService
    {
        Task<GeneralResponse> Register(UserDto userDto);
        Task<LoginResponse> Login(LoginDto loginDto);
        Task<LoginResponse> RefreshToken(AuthResponseDto request);
    }
}
