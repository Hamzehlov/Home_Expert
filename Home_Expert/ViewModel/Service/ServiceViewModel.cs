using Home_Expert.Models;

namespace Home_Expert.ViewModel.Service
{
    public class ServiceViewModel
    {
        public int Id { get; set; }

        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;

        public int TypeId { get; set; }
        public int? CategoryId { get; set; }

        public byte[]? Image { get; set; }

        // لإظهار القوائم في Select
        public List<Code> Types { get; set; } = new List<Code>();
        public List<Code> Categories { get; set; } = new List<Code>();

        // رفع الصورة
        public IFormFile? ImageFile { get; set; }



        public int TotalServices { get; set; }
        public int ActiveServices { get; set; }
        public int InactiveServices { get; set; }
    }
}
