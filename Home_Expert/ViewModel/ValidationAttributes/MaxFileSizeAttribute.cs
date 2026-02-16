using System.ComponentModel.DataAnnotations;

namespace Home_Expert.ViewModels.ValidationAttributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(
                        $"حجم الملف يجب أن لا يتجاوز {_maxFileSize / 1024 / 1024} ميجابايت");
                }
            }

            return ValidationResult.Success;
        }
    }
}