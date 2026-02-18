using Home_Expert.ViewModels.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Home_Expert.ViewModel.RegisterVendorDto
{
    public class RegisterVendorViewModel
    {
        [Required(ErrorMessage = "Error_FirstArNameRequired")]
        [StringLength(50, ErrorMessage = "Error_FirstNameLength")]
        [Display(Name = "Label_FirstNameAr")]
        public string FirstNameAr { get; set; }

        [Required(ErrorMessage = "Error_FirsEntNameRequired")]
        [StringLength(50, ErrorMessage = "Error_FirstNameLength")]
        [Display(Name = "Label_FirstNameEn")]
        public string FirstNameEn { get; set; }

     

        [Required(ErrorMessage = "Error_EmailRequired")]
        [EmailAddress(ErrorMessage = "Error_EmailInvalid")]
        [Display(Name = "Label_Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Error_PasswordRequired")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Error_PasswordLength")]
        [StrongPassword(ErrorMessage = "Error_PasswordWeak")]
        [DataType(DataType.Password)]
        [Display(Name = "Label_Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Error_ConfirmPasswordRequired")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Error_PasswordMismatch")]
        [Display(Name = "Label_Password")]
        public string ConfirmPassword { get; set; }

        [StringLength(10, ErrorMessage = "Error_PhoneLength")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Error_PhoneDigitsOnly")] // فقط 10 أرقام
        [Phone(ErrorMessage = "Error_PhoneInvalid")]
        [Display(Name = "Label_Phone")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Error_CompanyNameEnRequired")]
        [StringLength(100, ErrorMessage = "Error_CompanyNameLength")]
        [Display(Name = "Label_CompanyNameEn")]
        public string CompanyNameEn { get; set; }

        [Required(ErrorMessage = "Error_CompanyNameArRequired")]
        [StringLength(100, ErrorMessage = "Error_CompanyNameLength")]
        [Display(Name = "Label_CompanyNameAr")]
        public string CompanyNameAr { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Error_DescriptionLength")]
        [Display(Name = "Label_DescriptionEn")]
        public string? DescriptionEn { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Error_DescriptionLength")]
        [Display(Name = "Label_DescriptionAr")]
        public string? DescriptionAr { get; set; }

        [Range(0, 50, ErrorMessage = "Error_YearsExperienceRange")]
        [Display(Name = "Label_YearsExperience")]
        public int? YearsExperience { get; set; }

        [Required(ErrorMessage = "Error_ServiceTypeRequired")]
        [Display(Name = "Label_ServiceType")]
        public int ServiceTypeId { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Label_Logo")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [MaxFileSize(2 * 1024 * 1024)]
        public IFormFile? Logo { get; set; }

        [StringLength(250, ErrorMessage = "Error_ShowroomAddressLength")]
        [Display(Name = "Label_ShowroomAddressAr")]
        public string? ShowroomAddressAr { get; set; }

        [StringLength(250, ErrorMessage = "Error_ShowroomAddressLength")]
        [Display(Name = "Label_ShowroomAddressEn")]
        public string? ShowroomAddressEn { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Label_ShowroomImage")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile? ShowroomImage { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Label_CommercialFile")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".pdf" })]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile? CommercialRegistrationFile { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Label_WorkLicense")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".pdf" })]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile? WorkLicenseFile { get; set; }
    }
}
