namespace AutenticationBlazorWebApi.Models.DTOs
{
    public class UserRoleDto
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }

        public RoleDto Role { get; set; }
    }
}
