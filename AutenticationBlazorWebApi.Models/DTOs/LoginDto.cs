using CustomValidationAnnotation;
using System.ComponentModel.DataAnnotations;

namespace AutenticationBlazorWebApi.Models.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Email requerido")]
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