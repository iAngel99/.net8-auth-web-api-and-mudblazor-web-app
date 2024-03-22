using System.ComponentModel.DataAnnotations;

namespace AutenticationBlazorWebApi.Models.DTOs
{
    public class UserDto
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "El Nombre es requerido")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El Apellido es requerido")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email requerido")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "El Email no es válido")]
        public string Email { get; set; }

        public ICollection<UserRoleDto>? UserRole { get; set; }

    }
}
