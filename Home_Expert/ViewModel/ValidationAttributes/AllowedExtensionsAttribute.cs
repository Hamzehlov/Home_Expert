using System.ComponentModel.DataAnnotations;

namespace Home_Expert.ViewModels.ValidationAttributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(
                        $"الملف يجب أن يكون من النوع: {string.Join(", ", _extensions)}");
                }
            }

            return ValidationResult.Success;
        }
    }
}