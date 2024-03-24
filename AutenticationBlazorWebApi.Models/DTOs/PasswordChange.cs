using System.ComponentModel.DataAnnotations;

namespace AutenticationBlazorWebApi.Models.DTOs
{
    public class PasswordChange : PasswordChangeDto
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "La contraseña y confirmación de contraseña no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
