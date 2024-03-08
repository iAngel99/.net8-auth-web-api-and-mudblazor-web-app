using System.ComponentModel.DataAnnotations;

namespace CustomValidationAnnotation;

public class LowerCaseAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || !((string)value).Any(char.IsLower))
            return new ValidationResult("La contraseña debe incluir al menos una letra minúscula.");

        return ValidationResult.Success;
    }
}