using AutenticationBlazorWebApi.Client.Helpers;
using AutenticationBlazorWebApi.Client.Services.Contracts;
using AutenticationBlazorWebApi.Models.Responses;

namespace AutenticationBlazorWebApi.Client.Services.Implementations;


public class RoleService : IRoleService
{
    private readonly GetHttpClient _getHttpClient;
    public const string RoleUrl = "api/Role";

    public RoleService(GetHttpClient getHttpClient)
    {
        _getHttpClient = getHttpClient;
    }

    public async Task<GeneralListResponse<string>> GetAllRolesAsync()
    {
        var httpClient = _getHttpClient.GetPublicHttpClient();
        try
        {
            var result = await httpClient.GetFromJsonAsync<IEnumerable<string>>($"{RoleUrl}");
            // Asegúrate de convertir el resultado a List<string> para que coincida con el tipo esperado por GeneralListResponse<T>
            return new GeneralListResponse<string>(true, "", result.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new GeneralListResponse<string>(false, e.Message);
        }
    }
    public async Task<StringIdResponse> GetRoleIdByNameAsync(string roleName)
    {
        var httpClient = _getHttpClient.GetPublicHttpClient();
        try
        {
            // Assuming the backend service expects the role name as a path parameter
            var result = await httpClient.GetStringAsync($"{RoleUrl}/{roleName}");
            return new StringIdResponse(true, "", result.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // Consider handling different types of exceptions differently, e.g., returning null or throwing a custom exception
            return new StringIdResponse(false, e.Message);
        }
    }

}
