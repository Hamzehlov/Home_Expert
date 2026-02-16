using Home_Expert.ViewModels.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Home_Expert.ViewModel.RegisterVendorDto
{
    public class RegisterVendorViewModel
    {  
        
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(50, ErrorMessage = "الاسم الأول يجب أن لا يتجاوز 50 حرف")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [StringLength(50, ErrorMessage = "الاسم الأخير يجب أن لا يتجاوز 50 حرف")]
        [Display(Name = "الاسم الأخير")]
        public string LastName { get; set; }

        
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقين")]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; }

        [StringLength(20, ErrorMessage = "رقم الهاتف يجب أن لا يتجاوز 20 رقم")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string? Phone { get; set; }




        [Required(ErrorMessage = "اسم الشركة مطلوب")]
        [StringLength(100, ErrorMessage = "اسم الشركة يجب أن لا يتجاوز 100 حرف")]
        [Display(Name = "اسم الشركة")]
        public string CompanyName { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "الوصف يجب أن لا يتجاوز 500 حرف")]
        [Display(Name = "وصف الشركة")]
        public string? Description { get; set; }

        [Range(0, 50, ErrorMessage = "سنوات الخبرة يجب أن تكون بين 0 و 50")]
        [Display(Name = "سنوات الخبرة")]
        public int? YearsExperience { get; set; }

        [Required(ErrorMessage = "نوع الخدمة مطلوب")]
        [Display(Name = "نوع الخدمة")]
        public int ServiceTypeId { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "شعار الشركة")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [MaxFileSize(2 * 1024 * 1024)] // 2MB
        public IFormFile? Logo { get; set; }

    }
}
