using System.ComponentModel.DataAnnotations;

namespace Home_Expert.ViewModel.Service
{
    public class CodeViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string DescCodeAr { get; set; } = null!;

        [Required, StringLength(100)]
        public string DescCodeEn { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public int ParentId { get; set; } = 7; 
    }
}
