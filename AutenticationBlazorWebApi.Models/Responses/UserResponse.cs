using AutenticationBlazorWebApi.Models.DTOs;

namespace AutenticationBlazorWebApi.Models.Responses
{
    public record UserResponse(bool Flag, string Message = null!, UserDto UserDto = null!);
}
