using AutenticationBlazorWebApi.Models.Responses;

namespace AutenticationBlazorWebApi.Client.Services.Contracts;

public interface IRoleService
{
    Task<GeneralListResponse<string>> GetAllRolesAsync();

    Task<StringIdResponse> GetRoleIdByNameAsync(string roleName);
}