using Home_Expert.Models;
using System.ComponentModel.DataAnnotations;

namespace Home_Expert.ViewModel.Service
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required, StringLength(100)]
        public string NameAr { get; set; } = null!;

        [Required, StringLength(100)]
        public string NameEn { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? PriceRangeMin { get; set; }
        public decimal? PriceRangeMax { get; set; }

        public bool IsActive { get; set; } = true;

        public List<IFormFile>? ProductImages { get; set; }


        // اسم الصورة الرئيسية المحددة
        public string? MainImageFileName { get; set; }

        // قوائم للاختيار
        public List<Category> Categories { get; set; } = new List<Category>();


        // عدد المنتجات الإجمالي
        public int TotalProducts { get; set; }

        // عدد المنتجات النشطة
        public int ActiveProducts { get; set; }

        // عدد المنتجات غير النشطة
        public int InactiveProducts { get; set; }
    }
}
