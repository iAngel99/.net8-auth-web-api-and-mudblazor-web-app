using System.ComponentModel.DataAnnotations;

namespace CustomValidationAnnotation;

public class NumericDigitAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || !((string)value).Any(char.IsDigit))
            return new ValidationResult("La contraseña debe incluir al menos un dígito numérico.");

        return ValidationResult.Success;
    }
}