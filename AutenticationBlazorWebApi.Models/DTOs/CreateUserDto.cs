using CustomValidationAnnotation;
using System.ComponentModel.DataAnnotations;

namespace AutenticationBlazorWebApi.Models.DTOs
{
    public class CreateUserDto
    {
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

        [Required(ErrorMessage = "Contraseña requerida")]
        [StringLength(15, ErrorMessage = "Tu Contraseña debe tener de {2} a {1} characters", MinimumLength = 6)]
        [UpperCase]
        [LowerCase]
        [NumericDigit]
        [SpecialCharacter]
        public string Password { get; set; }
    }
}
