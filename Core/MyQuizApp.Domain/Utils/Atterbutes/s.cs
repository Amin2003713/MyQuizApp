using System.ComponentModel.DataAnnotations;

namespace MyQuizApp.Domain.Utils.Atterbutes;

public class DateGreaterThanAttribute(string comparisonProperty) : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var currentValue = (DateTime?)value;

        var property = validationContext.ObjectType.GetProperty(comparisonProperty);
        if (property == null)
        {
            throw new ArgumentException("Property with this name not found.");
        }

        var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

        if (currentValue.HasValue && comparisonValue.HasValue && currentValue <= comparisonValue)
        {
            return new ValidationResult(ErrorMessage ??
                                        $"{validationContext.DisplayName} must be greater than {comparisonProperty}.");
        }

        return ValidationResult.Success!;
    }
}