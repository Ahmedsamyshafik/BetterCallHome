using Domin.Helpers;
using System.ComponentModel.DataAnnotations;
namespace Infrastructure.CustomValidation
{
    public class GenderValidation : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            string? Gender = value.ToString().ToLower();
            if (Gender == null) { return new ValidationResult("Gender must be only Female or male!"); }
            if (Gender == GenderEnum.female.ToString() || Gender == GenderEnum.male.ToString())
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Gender must be only Female or male!");


        }
    }
}

