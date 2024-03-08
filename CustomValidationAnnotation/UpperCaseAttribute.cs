using System.ComponentModel.DataAnnotations;

namespace CustomValidationAnnotation;

public class UpperCaseAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || !((string)value).Any(char.IsUpper))
            return new ValidationResult("La contraseña debe incluir al menos una letra mayúscula.");

        return ValidationResult.Success;
    }
}