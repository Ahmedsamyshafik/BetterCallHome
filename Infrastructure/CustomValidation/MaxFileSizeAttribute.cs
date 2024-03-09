using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.CustomValidation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSize;

        public MaxFileSizeAttribute(long maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is ICollection<IFormFile> files)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > _maxFileSize)
                        {
                            return new ValidationResult("");
                        }
                    }
                }
                else
                {
                    var file = value as IFormFile;
                    if (file.Length > _maxFileSize)
                    {
                        return new ValidationResult("");
                    }
                }

            }
            return ValidationResult.Success;
        }
    }
}
