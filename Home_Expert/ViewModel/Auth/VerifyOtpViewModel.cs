using System.ComponentModel.DataAnnotations;

namespace Home_Expert.ViewModel.Auth
{
    public class VerifyOtpViewModel
    {
        [Required(ErrorMessage = "رمز التحقق مطلوب")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "رمز التحقق يجب أن يكون 6 أرقام")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "رمز التحقق يجب أن يحتوي على أرقام فقط")]
        [Display(Name = "رمز التحقق")]
        public string OtpCode { get; set; }
        
        public string Email { get; set; } = null!;

    }
}
