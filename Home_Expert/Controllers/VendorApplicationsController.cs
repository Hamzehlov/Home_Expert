using Home_Expert.Models;
using Home_Expert.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
 using MimeKit;
namespace Home_Expert.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VendorApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _sharedResources;
        private readonly IConfiguration _config;

        public VendorApplicationsController(
            ApplicationDbContext context,
            IStringLocalizer<SharedResource> sharedResources,
            IConfiguration config)
        {
            _context = context;
            _sharedResources = sharedResources;
            _config = config;
        }

        // ════════════════════════════════════════════════
        //  GET: /VendorApplications
        // ════════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            var vendors = await _context.Vendors
                .Include(v => v.ServiceType)
                .Include(v => v.User)
                .OrderByDescending(v => v.Id)
                .ToListAsync();

            return View(vendors);
        }

       

        // ════════════════════════════════════════════════
        //  POST: /VendorApplications/Approve
        // ════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var vendor = await _context.Vendors
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vendor == null)
            {
                TempData["Error"] = _sharedResources[SharedResourcesKeys.Message_VendorRejected].Value;
                return RedirectToAction(nameof(Index));
            }

            vendor.Verified = 1;
            vendor.RejectionReasonAr = null;
            vendor.RejectionReasonEn = null;
            await _context.SaveChangesAsync();

            // Send approval email
            var isArabic = System.Globalization.CultureInfo.CurrentUICulture.Name.StartsWith("ar");
            var companyName = isArabic ? vendor.CompanyNameAr : vendor.CompanyNameEn;

            await SendEmailAsync(
                toEmail: vendor.User.Email!,
                toName: companyName,
                subject: isArabic
                    ? $"تهانينا! تم قبول طلب انضمامكم – {companyName}"
                    : $"Congratulations! Your application has been approved – {companyName}",
                body: BuildApprovalEmailBody(vendor, isArabic)
            );

            TempData["Success"] = _sharedResources[SharedResourcesKeys.Message_VendorAccepted].Value
                                + " — " + _sharedResources[SharedResourcesKeys.Message_EmailSent].Value;
            return RedirectToAction(nameof(Index));
        }

        // ════════════════════════════════════════════════
        //  POST: /VendorApplications/Reject
        // ════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string rejectionReasonAr, string rejectionReasonEn)
        {
            if (string.IsNullOrWhiteSpace(rejectionReasonAr) && string.IsNullOrWhiteSpace(rejectionReasonEn))
            {
                TempData["Error"] = _sharedResources[SharedResourcesKeys.Rejection_Reason].Value;
                return RedirectToAction(nameof(Index));
            }

            var vendor = await _context.Vendors
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vendor == null)
            {
                TempData["Error"] = _sharedResources[SharedResourcesKeys.Message_VendorRejected].Value;
                return RedirectToAction(nameof(Index));
            }

            vendor.Verified = 2;
            vendor.RejectionReasonAr = rejectionReasonAr?.Trim();
            vendor.RejectionReasonEn = rejectionReasonEn?.Trim();
            await _context.SaveChangesAsync();

            // Send rejection email — use current culture for email body
            var isArabic = System.Globalization.CultureInfo.CurrentUICulture.Name.StartsWith("ar");
            var companyName = isArabic ? vendor.CompanyNameAr : vendor.CompanyNameEn;

            await SendEmailAsync(
                toEmail: vendor.User.Email!,
                toName: companyName,
                subject: isArabic
                    ? $"بخصوص طلب انضمامكم – {companyName}"
                    : $"Regarding your application – {companyName}",
                body: BuildRejectionEmailBody(vendor, isArabic)
            );

            TempData["Success"] = _sharedResources[SharedResourcesKeys.Message_VendorRejected].Value
                                + " — " + _sharedResources[SharedResourcesKeys.Message_EmailSent].Value;
            return RedirectToAction(nameof(Index));
        }

        // ════════════════════════════════════════════════
        //  Approval Email HTML
        // ════════════════════════════════════════════════
        private static string BuildApprovalEmailBody(Vendor vendor, bool isArabic)
        {
            var name = isArabic ? vendor.CompanyNameAr : vendor.CompanyNameEn;
            var dir = isArabic ? "rtl" : "ltr";

            if (isArabic) return $@"
<div dir=""{dir}"" style=""font-family:Tahoma,Arial,sans-serif;max-width:580px;margin:auto;background:#fff;border-radius:12px;overflow:hidden;border:1px solid #e2e8f0;"">
  <div style=""background:#0A3D44;padding:28px 32px;text-align:center;""><h1 style=""color:#fff;margin:0;font-size:1.4rem;"">Home Expert</h1></div>
  <div style=""padding:32px;"">
    <h2 style=""color:#0A3D44;margin-top:0;"">🎉 تهانينا، {name}!</h2>
    <p style=""color:#374151;line-height:1.8;"">يسعدنا إخباركم بأنه <strong>تمت الموافقة على طلب انضمامكم</strong> إلى منصة Home Expert.</p>
    <p style=""color:#374151;line-height:1.8;"">يمكنكم الآن تسجيل الدخول إلى حسابكم وبدء إضافة منتجاتكم وخدماتكم.</p>
    <div style=""background:#ecfdf5;border-radius:8px;padding:16px;margin:20px 0;"">
      <p style=""margin:0;color:#065f46;font-weight:600;"">✅ حسابكم مفعّل وجاهز للاستخدام</p>
    </div>
    <p style=""color:#6b7280;font-size:.85rem;"">شكراً لاختياركم Home Expert — نتطلع للعمل معكم.</p>
  </div>
  <div style=""background:#f8fafc;padding:16px 32px;text-align:center;color:#9ca3af;font-size:.78rem;"">© {DateTime.Now.Year} Home Expert. جميع الحقوق محفوظة.</div>
</div>";

            return $@"
<div dir=""{dir}"" style=""font-family:Arial,sans-serif;max-width:580px;margin:auto;background:#fff;border-radius:12px;overflow:hidden;border:1px solid #e2e8f0;"">
  <div style=""background:#0A3D44;padding:28px 32px;text-align:center;""><h1 style=""color:#fff;margin:0;font-size:1.4rem;"">Home Expert</h1></div>
  <div style=""padding:32px;"">
    <h2 style=""color:#0A3D44;margin-top:0;"">🎉 Congratulations, {name}!</h2>
    <p style=""color:#374151;line-height:1.8;"">We are pleased to inform you that your application to join <strong>Home Expert</strong> has been <strong>approved</strong>.</p>
    <p style=""color:#374151;line-height:1.8;"">You can now log in to your account and start adding your products and services.</p>
    <div style=""background:#ecfdf5;border-radius:8px;padding:16px;margin:20px 0;"">
      <p style=""margin:0;color:#065f46;font-weight:600;"">✅ Your account is now active and ready to use.</p>
    </div>
    <p style=""color:#6b7280;font-size:.85rem;"">Thank you for choosing Home Expert — we look forward to working with you.</p>
  </div>
  <div style=""background:#f8fafc;padding:16px 32px;text-align:center;color:#9ca3af;font-size:.78rem;"">© {DateTime.Now.Year} Home Expert. All rights reserved.</div>
</div>";
        }

        // ════════════════════════════════════════════════
        //  Rejection Email HTML — sends BOTH reasons if available
        // ════════════════════════════════════════════════
        private static string BuildRejectionEmailBody(Vendor vendor, bool isArabic)
        {
            var name = isArabic ? vendor.CompanyNameAr : vendor.CompanyNameEn;
            var dir = isArabic ? "rtl" : "ltr";
            var reason = isArabic
                ? (vendor.RejectionReasonAr ?? vendor.RejectionReasonEn ?? "-")
                : (vendor.RejectionReasonEn ?? vendor.RejectionReasonAr ?? "-");

            if (isArabic) return $@"
<div dir=""{dir}"" style=""font-family:Tahoma,Arial,sans-serif;max-width:580px;margin:auto;background:#fff;border-radius:12px;overflow:hidden;border:1px solid #e2e8f0;"">
  <div style=""background:#0A3D44;padding:28px 32px;text-align:center;""><h1 style=""color:#fff;margin:0;font-size:1.4rem;"">Home Expert</h1></div>
  <div style=""padding:32px;"">
    <h2 style=""color:#dc2626;margin-top:0;"">بخصوص طلب انضمامكم</h2>
    <p style=""color:#374151;line-height:1.8;"">عزيزنا <strong>{name}</strong>،</p>
    <p style=""color:#374151;line-height:1.8;"">شكراً لتقديمكم طلب الانضمام إلى منصة Home Expert. بعد مراجعة طلبكم، نأسف لإخباركم بأنه <strong>لم تتم الموافقة</strong> على الطلب في الوقت الحالي.</p>
    <div style=""background:#fef2f2;border:1px solid #fca5a5;border-radius:8px;padding:16px;margin:20px 0;"">
      <p style=""margin:0 0 6px;color:#991b1b;font-weight:700;"">❌ سبب الرفض:</p>
      <p style=""margin:0;color:#7f1d1d;line-height:1.7;"">{System.Web.HttpUtility.HtmlEncode(reason)}</p>
    </div>
    <p style=""color:#374151;line-height:1.8;"">يمكنكم التواصل معنا لمزيد من المعلومات أو إعادة التقديم بعد معالجة الملاحظات.</p>
  </div>
  <div style=""background:#f8fafc;padding:16px 32px;text-align:center;color:#9ca3af;font-size:.78rem;"">© {DateTime.Now.Year} Home Expert. جميع الحقوق محفوظة.</div>
</div>";

            return $@"
<div dir=""{dir}"" style=""font-family:Arial,sans-serif;max-width:580px;margin:auto;background:#fff;border-radius:12px;overflow:hidden;border:1px solid #e2e8f0;"">
  <div style=""background:#0A3D44;padding:28px 32px;text-align:center;""><h1 style=""color:#fff;margin:0;font-size:1.4rem;"">Home Expert</h1></div>
  <div style=""padding:32px;"">
    <h2 style=""color:#dc2626;margin-top:0;"">Regarding Your Application</h2>
    <p style=""color:#374151;line-height:1.8;"">Dear <strong>{name}</strong>,</p>
    <p style=""color:#374151;line-height:1.8;"">Thank you for applying to join Home Expert. After reviewing your application, we regret to inform you that it has <strong>not been approved</strong> at this time.</p>
    <div style=""background:#fef2f2;border:1px solid #fca5a5;border-radius:8px;padding:16px;margin:20px 0;"">
      <p style=""margin:0 0 6px;color:#991b1b;font-weight:700;"">❌ Reason for Rejection:</p>
      <p style=""margin:0;color:#7f1d1d;line-height:1.7;"">{System.Web.HttpUtility.HtmlEncode(reason)}</p>
    </div>
    <p style=""color:#374151;line-height:1.8;"">You are welcome to contact us or reapply after addressing the points above.</p>
  </div>
  <div style=""background:#f8fafc;padding:16px 32px;text-align:center;color:#9ca3af;font-size:.78rem;"">© {DateTime.Now.Year} Home Expert. All rights reserved.</div>
</div>";
        }

        // ════════════════════════════════════════════════
        //  SMTP Email Sender (appsettings.json)
        // ════════════════════════════════════════════════
        private async Task SendEmailAsync(string toEmail, string toName, string subject, string body)
        {
            var smtpHost = _config["EmailSettings:SmtpServer"];
            var smtpPortStr = _config["EmailSettings:SmtpPort"];
            var smtpUser = _config["EmailSettings:SmtpUsername"];
            var smtpPass = _config["EmailSettings:SmtpPassword"];
            var fromDisplay = _config["EmailSettings:SenderName"] ?? "Home Expert";

            if (string.IsNullOrWhiteSpace(smtpHost) ||
                string.IsNullOrWhiteSpace(smtpUser) ||
                string.IsNullOrWhiteSpace(smtpPass) ||
                string.IsNullOrWhiteSpace(toEmail))
                return;

            var smtpPort = int.TryParse(smtpPortStr, out var p) ? p : 587;

            // بناء الرسالة
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromDisplay, smtpUser));
            message.To.Add(new MailboxAddress(toName ?? toEmail, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new MailKit.Net.Smtp.SmtpClient();

            var secureOption = smtpPort == 465
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTls;

            await client.ConnectAsync(smtpHost, smtpPort, secureOption);
            await client.AuthenticateAsync(smtpUser, smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        // ════════════════════════════════════════════════
        //  GET: /VendorApplications/Details
        // ════════════════════════════════════════════════
        public async Task<IActionResult> Details(int id)
        {
            var vendor = await _context.Vendors
                .Include(v => v.ServiceType)
                .Include(v => v.User)
                .Include(v => v.VendorMedia)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vendor == null) return NotFound();
            return View(vendor);
        }
    }
}
