using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;

namespace Home_Expert.Services
{
    // ✅ أضف : IOtpService هنا
    public class OtpService : IOtpService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OtpService> _logger;

        public OtpService(IConfiguration configuration, ILogger<OtpService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

  
public async Task<bool> SendOtpEmail(string email, string otp)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Home Expert", _configuration["EmailSettings:SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "رمز التحقق (OTP)";
            message.Body = new TextPart("plain") { Text = $"رمز التحقق الخاص بك هو: {otp}\nصالح لمدة 5 دقائق." };

                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
                                      int.Parse(_configuration["EmailSettings:SmtpPort"]),
                                      MailKit.Security.SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(_configuration["EmailSettings:SmtpUsername"],
                                           _configuration["EmailSettings:SmtpPassword"]);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            Console.WriteLine($"✅ OTP sent to {email}: {otp}");
            return true;
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending OTP to {Email}", email);
                Console.WriteLine($"❌ Error sending OTP to {email}: {ex}");
                return false;
            }

        }


        public bool ValidateOtp(string userOtp, string storedOtp, DateTime? expiry)
        {
            // 1. التحقق من وجود OTP
            if (string.IsNullOrEmpty(storedOtp) || !expiry.HasValue)
            {
                _logger?.LogWarning("OTP validation failed: No stored OTP or expiry");
                return false;
            }

            // 2. التحقق من انتهاء الصلاحية
            if (DateTime.UtcNow > expiry.Value)
            {
                _logger?.LogWarning("OTP validation failed: OTP expired");
                return false;
            }

            // 3. التحقق من تطابق الكود
            bool isValid = userOtp == storedOtp;

            if (isValid)
            {
                _logger?.LogInformation("✅ OTP validated successfully");
            }
            else
            {
                _logger?.LogWarning("❌ OTP validation failed: Code mismatch");
            }

            return isValid;
        }
    }
}