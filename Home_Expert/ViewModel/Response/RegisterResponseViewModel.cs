// ViewModels/Response/RegisterResponseViewModel.cs
namespace Home_Expert.ViewModels.Response
{
    public class RegisterResponseViewModel
    {
        public string Email { get; set; }
        public DateTime OtpExpiresAt { get; set; }
        public string Message { get; set; }
    }
}