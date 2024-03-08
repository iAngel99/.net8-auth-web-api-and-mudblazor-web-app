using System.ComponentModel.DataAnnotations;

namespace CustomValidationAnnotation;

public class SpecialCharacterAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || !((string)value).Any(ch => !char.IsLetterOrDigit(ch)))
            return new ValidationResult("La contraseña debe incluir al menos un caracter especial.");

        return ValidationResult.Success;
    }
}