using System.ComponentModel.DataAnnotations;

namespace Infrastructure.CustomValidation
{
    public class AgeValidation : ValidationAttribute
    {
        static bool IsNumeric(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            return input.All(char.IsDigit);
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            string? Age = value.ToString();




            bool checkingAlphapitcs = IsNumeric(Age);
            if (checkingAlphapitcs)
            {
                int realAge = int.Parse(Age);

                if (realAge <= 16)
                {
                    return new ValidationResult("Age Must be Above or Equal 16");

                }
                return ValidationResult.Success;
            }
            return new ValidationResult("ONLY Numbers Please.. ");


        }
    }
}
