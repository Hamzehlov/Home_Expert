using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Home_Expert.ViewModels.ValidationAttributes
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Required attribute will handle this
            }

            string password = value.ToString();

            // At least one uppercase letter
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult(GetErrorMessage("uppercase"));
            }

            // At least one lowercase letter
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return new ValidationResult(GetErrorMessage("lowercase"));
            }

            // At least one digit
            if (!Regex.IsMatch(password, @"\d"))
            {
                return new ValidationResult(GetErrorMessage("digit"));
            }

            // At least one special character
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?]"))
            {
                return new ValidationResult(GetErrorMessage("special"));
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(string type)
        {
            // Will be overridden by ErrorMessage in the attribute
            return ErrorMessage ?? $"Password must contain at least one {type} character";
        }
    }
}