namespace Home_Expert.ViewModels.Response
{
    public class VerifyOtpResponseViewModel
    {
        public string UserId { get; set; }
        public int VendorId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Token { get; set; } 
    }
}