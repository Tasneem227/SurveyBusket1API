using System.ComponentModel.DataAnnotations;
namespace SurveyBusket8.ValidationAttributes;

[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]

public class MinAgeAttribute : ValidationAttribute {
    public override bool IsValid(object? value)
    {
        if (value is not null)
        {
            var date = (DateTime)value;
            if (DateTime.Today < date.AddYears(18))
            {
                return false; 
            }
            return true;
        }
        return false;
    }
}
    