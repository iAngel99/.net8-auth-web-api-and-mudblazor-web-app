using Microsoft.AspNetCore.Identity;

namespace AutenticationBlazorWebApi.Models
{
    public class Role : IdentityRole
    {
        public Role(string roleName) : base(roleName)
        {
        }

        public Role()
        {
        }

        public ICollection<UserRole>? UserRole { get; set; }
    }
}
