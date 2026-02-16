// Services/IOtpService.cs
using Home_Expert.Models;
using Home_Expert.Services;

namespace Home_Expert.Services
{
    public interface IOtpService
    {
        string GenerateOtp();
        Task<bool> SendOtpEmail(string email, string otp);
        bool ValidateOtp(string userOtp, string storedOtp, DateTime? expiry);
    }
}
