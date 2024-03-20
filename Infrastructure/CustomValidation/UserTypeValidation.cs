using Domin.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.CustomValidation
{
    public class UserTypeValidation : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var Type = value;
            if (Type == null) return new ValidationResult("Type must be only User or Owner!");
            if (Type.ToString() == TypeEnum.owner.ToString() || Type.ToString() == TypeEnum.user.ToString())
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Type must be only User or Owner!");


        }
    }
}
