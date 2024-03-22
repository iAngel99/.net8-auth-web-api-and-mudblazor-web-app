using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AutenticationBlazorWebApi.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El Apellido es requerido")]
        public string LastName { get; set; }

        public ICollection<UserRole>? UserRole { get; set; }

    }
}
