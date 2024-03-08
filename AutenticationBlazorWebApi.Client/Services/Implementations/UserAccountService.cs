using AutenticationBlazorWebApi.Client.Helpers;
using AutenticationBlazorWebApi.Client.Services.Contracts;
using AutenticationBlazorWebApi.Models.DTOs;
using AutenticationBlazorWebApi.Models.Responses;

namespace AutenticationBlazorWebApi.Client.Services.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly GetHttpClient _gethttpClient;

        public const string AuthUrl = "api/Account";

        public UserAccountService(GetHttpClient gethttpClient)
        {
            _gethttpClient = gethttpClient;
        }

        public async Task<GeneralResponse> Register(UserDto userDto)
        {
            var httpClient = _gethttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/register", userDto);
                if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Error occured");

                return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new GeneralResponse(false, "Error occured");
            }
        }

        public async Task<LoginResponse> Login(LoginDto loginDto)
        {
            var httpClient = _gethttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/login", loginDto);
                if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");
                return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new LoginResponse(false, "Error occured");
            }
        }

        public async Task<LoginResponse> RefreshToken(AuthResponseDto request)
        {
            var httpClient = _gethttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/refreshtoken", request);
                if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");

                return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new LoginResponse(false, "Error occured");
            }
        }
    }
}
