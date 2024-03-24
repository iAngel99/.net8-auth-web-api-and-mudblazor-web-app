using AutenticationBlazorWebApi.Client.Helpers;
using AutenticationBlazorWebApi.Client.Services.Contracts;
using AutenticationBlazorWebApi.Models.DTOs;
using AutenticationBlazorWebApi.Models.Responses;

namespace AutenticationBlazorWebApi.Client.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly GetHttpClient _getHttpClient;
        public const string UserUrl = "api/User";

        public UserService(GetHttpClient getHttpClient)
        {
            _getHttpClient = getHttpClient;
        }

        public async Task<GeneralListResponse<UserDto>> GetAllUsersAsync()
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.GetFromJsonAsync<IEnumerable<UserDto>>($"{UserUrl}");
                return new GeneralListResponse<UserDto>(true, "", result.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new GeneralListResponse<UserDto>(false, "Ocurrió un error");
            }
        }

        public async Task<UserResponse> GetUserByIdAsync(string id)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.GetFromJsonAsync<UserResponse>($"{UserUrl}/{id}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new UserResponse(false, "Ocurrió un error");
            }
        }

        public async Task<UserResponse> GetUserByEmailAsync(string email)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.GetFromJsonAsync<UserResponse>($"{UserUrl}/ByEmail/{email}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new UserResponse(false, "Ocurrió un error");
            }
        }

        public async Task<UserResponse> AddUserAsync(CreateUserDto user)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.PostAsJsonAsync($"{UserUrl}", user);
                if (!result.IsSuccessStatusCode) return new UserResponse(false, "Ocurrió un error");

                return await result.Content.ReadFromJsonAsync<UserResponse>()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new UserResponse(false, "Ocurrió un error");
            }
        }

        public async Task<GeneralResponse> UpdateUserAsync(string id, UserDto user)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.PutAsJsonAsync($"{UserUrl}/{id}", user);
                if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Ocurrió un error");

                return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new GeneralResponse(false, "Ocurrió un error");
            }
        }

        public async Task<GeneralResponse> DeleteUserAsync(string id)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.DeleteAsync($"{UserUrl}/{id}");
                if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Ocurrió un error");

                return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new GeneralResponse(false, "Ocurrió un error");
            }
        }

        public async Task<GeneralResponse> AddUserRoleAsync(UserRoleDto addUserRoleDto)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.PostAsJsonAsync($"{UserUrl}/AddUserRole", addUserRoleDto);
                if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Ocurrió un error");

                return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new GeneralResponse(false, "Ocurrió un error");
            }
        }

        public async Task<GeneralListResponse<string>> GetRolesAsync(string userId)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.GetFromJsonAsync<IList<string>>($"{UserUrl}/Roles/{userId}");
                return new GeneralListResponse<string>(true, "Exito", result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new GeneralListResponse<string>(false, "Ocurrió un error");
            }
        }

        public async Task<GeneralResponse> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {
                var result = await httpClient.DeleteAsync($"{UserUrl}/{userId}/roles/{roleName}");
                return new GeneralResponse(true, "Rol eliminado");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new GeneralResponse(false, "Ocurrió un error");
            }
        }

        public async Task<GeneralResponse> ChangePasswordAsync(string userId, PasswordChangeDto passwordChangeDto)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {

                var result = await httpClient.PostAsJsonAsync($"{UserUrl}/changepassword/{userId}", passwordChangeDto);
                if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Ocurrió un error");

                return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new GeneralResponse(false, "Ocurrió un error");
            }
        }

        public async Task<GeneralResponse> AdminChangePasswordAsync(string userId, string newPassword)
        {
            var httpClient = _getHttpClient.GetPublicHttpClient();
            try
            {

                var result = await httpClient.PostAsJsonAsync($"{UserUrl}/adminchangepassword/{userId}", newPassword);
                if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Ocurrió un error");

                return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new GeneralResponse(false, "Ocurrió un error");
            }
        }
    }
}
