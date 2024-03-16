using System.ComponentModel.DataAnnotations;

namespace Infrastructure.CustomValidation
{
    public class PhoneValidation : ValidationAttribute
    {

        static bool IsNumeric(string input)
        {

            return input.All(char.IsDigit);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;
            if (phoneNumber == null) { return new ValidationResult("The Phone field is required."); }
            bool checkingAlphapitcs = IsNumeric(phoneNumber);
            if (checkingAlphapitcs)
            {
                // check 11 number..
                if (phoneNumber.Length != 11) return new ValidationResult($"11 number only is Allowed ..");

                return ValidationResult.Success;

            }
            return new ValidationResult($"ONLY Numbers Please.. ");


        }
    }
}
