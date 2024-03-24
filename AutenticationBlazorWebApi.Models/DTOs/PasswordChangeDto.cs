using CustomValidationAnnotation;
using System.ComponentModel.DataAnnotations;

namespace AutenticationBlazorWebApi.Models.DTOs
{
    public class PasswordChangeDto
    {
        [Required(ErrorMessage = "Contraseña actual requerida")]

        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Contraseña nueva requerida")]
        [StringLength(15, ErrorMessage = "Tu Contraseña debe tener de {2} a {1} characters", MinimumLength = 6)]
        [UpperCase]
        [LowerCase]
        [NumericDigit]
        [SpecialCharacter]
        public string Password { get; set; }


    }
}
