using AutenticationBlazorWebApi.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace AutenticationBlazorWebApi.Server.Repository
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDto> GetUserByEmailAsync(string userEmail);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> AddUserAsync(CreateUserDto userDto);
        Task<UserDto> UpdateUserAsync(UserDto userDto);
        Task DeleteUserAsync(string userId);
        Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName);
        Task<IList<string>> GetRolesAsync(string userId);

        Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleName);

        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        Task<IdentityResult> AdminChangePasswordAsync(string userId, string newPassword);


    }
}
