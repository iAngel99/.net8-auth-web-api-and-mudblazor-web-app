using AutenticationBlazorWebApi.Models.DTOs;
using AutenticationBlazorWebApi.Models.Responses;

namespace AutenticationBlazorWebApi.Client.Services.Contracts
{
    public interface IUserService
    {
        Task<GeneralListResponse<UserDto>> GetAllUsersAsync();
        Task<UserResponse> GetUserByIdAsync(string id);
        Task<UserResponse> GetUserByEmailAsync(string email);
        Task<UserResponse> AddUserAsync(CreateUserDto user);
        Task<GeneralResponse> UpdateUserAsync(string id, UserDto user);
        Task<GeneralResponse> DeleteUserAsync(string id);
        Task<GeneralResponse> AddUserRoleAsync(UserRoleDto addUserRoleDto);
        Task<GeneralListResponse<string>> GetRolesAsync(string userId);
        Task<GeneralResponse> RemoveUserFromRoleAsync(string userId, string roleName);

        Task<GeneralResponse> ChangePasswordAsync(string userId, PasswordChangeDto passwordChangeDto);

        Task<GeneralResponse> AdminChangePasswordAsync(string userId, string newPassword);
    }

}
