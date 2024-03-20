using Domin.Helpers;
using System.ComponentModel.DataAnnotations;
namespace Infrastructure.CustomValidation
{
    public class GenderValidation : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var Gender = value;
            if (Gender == null) { return new ValidationResult("Gender must be only Female or male!"); }
            if (Gender.ToString() == GenderEnum.female.ToString() || Gender.ToString() == GenderEnum.male.ToString())
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Gender must be only Female or male!");


        }
    }
}

