using CustomValidationAnnotation;
using System.ComponentModel.DataAnnotations;

namespace AutenticationBlazorWebApi.Client.Helpers
{
    public class ChangePasswordDialogParameters
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña nueva requerida")]
        [StringLength(15, ErrorMessage = "Tu Contraseña debe tener de {2} a {1} characters", MinimumLength = 6)]
        [UpperCase]
        [LowerCase]
        [NumericDigit]
        [SpecialCharacter]
        public string NewPassword { get; set; }
    }
}
