using System.ComponentModel.DataAnnotations;

namespace AutenticationBlazorWebApi.Models.DTOs
{
    public class RegisterDto : LoginDto
    {
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El Apellido es requerido")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "La contraseña y confirmación de contraseña no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
