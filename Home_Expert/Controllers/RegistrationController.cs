using Home_Expert.Models;
using Home_Expert.Resources;
using Home_Expert.Services;
using Home_Expert.ViewModel.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Text.Json;
namespace Home_Expert.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<RegistrationController> _logger;
        private readonly ApplicationDbContext _context; 
        private readonly IOtpService _otpService;

        public RegistrationController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<SharedResource> localizer,
            ILogger<RegistrationController> logger,
            ApplicationDbContext context, 
            IOtpService otpService
) 
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _localizer = localizer;
            _logger = logger;
            _context = context;
            _otpService = otpService;

        }

        // ==========================================
        // عرض صفحة التسجيل
        // ==========================================
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        // ==========================================
        // عرض صفحة تسجيل الدخول
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ==========================================
        // معالجة تسجيل الدخول
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = _localizer["Message_InvalidData"].Value;
                return View(model);
            }

            try
            {
                // 1️⃣ تحقق من وجود المستخدم
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["ErrorMessage"] = _localizer["Error_AccountNotFound"].Value;
                    TempData["ShowRegisterLink"] = true;
                    return View(model);
                }

                // ✅ 1.5️⃣ شيك على الـ Role
                var userRoles = await _userManager.GetRolesAsync(user);

                // إذا كان Customer بيحاول يدخل على صفحة Vendor login
                if (userRoles.Contains("User"))
                {
                    TempData["ErrorMessage"] = _localizer["Error_CustomerCannotLoginAsVendor"].Value;
                    return View(model);
                }

                // إذا كان Admin
                if (userRoles.Contains("Admin"))
                {
                    TempData["ErrorMessage"] = _localizer["Error_AdminLoginRestricted"].Value;
                    return View(model);
                }

                // إذا مش Vendor أساساً
                if (!userRoles.Contains("Vendor"))
                {
                    TempData["ErrorMessage"] = _localizer["Error_NotVendorAccount"].Value;
                    return View(model);
                }

                // 2️⃣ تحقق من حالة التوثيق من جدول Vendor
                if (await _userManager.IsInRoleAsync(user, "Vendor"))
                {
                    var vendor = await _context.Vendors.FirstOrDefaultAsync(v => v.UserId == user.Id);

                    if (vendor != null && vendor.Verified == 0)
                    {
                        // ✅ تحقق من كلمة المرور أولاً
                        var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (!passwordCheck)
                        {
                            TempData["ErrorMessage"] = _localizer["Error_WrongPassword"].Value;
                            return View(model);
                        }

                        TempData["PendingEmail"] = model.Email;
                        return RedirectToAction("PendingVerification", "Registration");
                    }

                    if (vendor != null && vendor.Verified == 2)
                    {
                        // ✅ تحقق من كلمة المرور أولاً
                        var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (!passwordCheck)
                        {
                            TempData["ErrorMessage"] = _localizer["Error_WrongPassword"].Value;
                            return View(model);
                        }

                        TempData["RejectedEmail"] = model.Email;
                        return RedirectToAction("RejectedVerification", "Registration");
                    }
                }

                // 3️⃣ محاولة تسجيل الدخول
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName!,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false
                );

                // 4️⃣ نجح تسجيل الدخول
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} logged in successfully", model.Email);

                    // التوجيه حسب الـ Role
                    if (await _userManager.IsInRoleAsync(user, "Vendor"))
                    {
                        return RedirectToAction("Index", "VendorDashboard");
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Customer"))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return LocalRedirect(returnUrl);
                }

                // 5️⃣ الحساب محظور
                if (result.IsLockedOut)
                {
                    TempData["ErrorMessage"] = _localizer["Error_AccountLocked"].Value;
                    return View(model);
                }

                // 6️⃣ كلمة المرور غير صحيحة
                TempData["ErrorMessage"] = _localizer["Error_WrongPassword"].Value;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", model.Email);
                TempData["ErrorMessage"] = _localizer["Message_ServerError"].Value;
                return View(model);
            }
        }

            // ==========================================
            // صفحة انتظار التحقق
            // ==========================================
            [HttpGet]
        public IActionResult PendingVerification()
        {
            var email = TempData["PendingEmail"] as string;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            ViewBag.Email = email;
            return View();
        }
        // ==========================================
        // صفحة  رفض
        // ==========================================
        [HttpGet]
        public IActionResult RejectedVerification()
        {
            var email = TempData["RejectedEmail"] as string;

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            ViewBag.Email = email;
            ViewBag.RejectionReason = TempData["RejectionReason"] as string;
            return View();
        }

        // ==========================================
        // تسجيل الخروج
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");
            return RedirectToAction("Login", "Registration");
        }

        // ==========================================
        // GET: صفحة نسيت كلمة المرور
        // ==========================================
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // ==========================================
        // POST: خطوة 1 - إرسال OTP
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordSendOtp(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["ForgotError"] = _localizer["Error_AccountNotFound"].Value;
                TempData["ForgotStep"] = "1";
                return RedirectToAction("ForgotPassword");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Vendor"))
            {
                TempData["ForgotError"] = _localizer["Error_NotVendorAccount"].Value;
                TempData["ForgotStep"] = "1";
                return RedirectToAction("ForgotPassword");
            }

            var otp = _otpService.GenerateOtp();
            var expiry = DateTime.UtcNow.AddMinutes(5);

            HttpContext.Session.SetString("ForgotPasswordData", JsonSerializer.Serialize(new
            {
                Email = email,
                OtpCode = otp,
                OtpExpiry = expiry,
                OtpAttempts = 0
            }));

            await _otpService.SendOtpEmail(email, otp);

            TempData["ForgotStep"] = "2";
            TempData["ForgotEmail"] = email;
            return RedirectToAction("ForgotPassword");
        }

        // ==========================================
        // POST: خطوة 2 - التحقق من OTP
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPasswordVerifyOtp(string otpCode)
        {
            var sessionStr = HttpContext.Session.GetString("ForgotPasswordData");
            if (string.IsNullOrEmpty(sessionStr))
            {
                TempData["ForgotError"] = _localizer["Message_SessionExpired"].Value;
                TempData["ForgotStep"] = "1";
                return RedirectToAction("ForgotPassword");
            }

            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(sessionStr)!;
            string storedOtp = data["OtpCode"].GetString()!;
            DateTime expiry = data["OtpExpiry"].GetDateTime();
            int attempts = data["OtpAttempts"].GetInt32();
            string email = data["Email"].GetString()!;

            bool isValid = _otpService.ValidateOtp(otpCode.Trim(), storedOtp.Trim(), expiry);

            if (!isValid)
            {
                attempts++;
                if (attempts >= 3)
                {
                    HttpContext.Session.Remove("ForgotPasswordData");
                    TempData["ForgotError"] = _localizer["Message_MaxAttemptsReached"].Value;
                    TempData["ForgotStep"] = "1";
                    return RedirectToAction("ForgotPassword");
                }

                data["OtpAttempts"] = JsonSerializer.SerializeToElement(attempts);
                HttpContext.Session.SetString("ForgotPasswordData", JsonSerializer.Serialize(data));

                TempData["ForgotError"] = string.Format(_localizer["Message_OTPIncorrect"].Value, 3 - attempts);
                TempData["ForgotStep"] = "2";
                TempData["ForgotEmail"] = email;
                return RedirectToAction("ForgotPassword");
            }

            HttpContext.Session.Remove("ForgotPasswordData");
            HttpContext.Session.SetString("ForgotPasswordVerified", email);

            TempData["ForgotStep"] = "3";
            TempData["ForgotEmail"] = email;
            return RedirectToAction("ForgotPassword");
        }

        // ==========================================
        // POST: خطوة 3 - تغيير كلمة المرور
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordReset(string newPassword, string confirmPassword)
        {
            var email = HttpContext.Session.GetString("ForgotPasswordVerified");
            if (string.IsNullOrEmpty(email))
            {
                TempData["ForgotStep"] = "1";
                return RedirectToAction("ForgotPassword");
            }

            if (newPassword != confirmPassword)
            {
                TempData["ForgotError"] = _localizer["Error_PasswordMismatch"].Value;
                TempData["ForgotStep"] = "3";
                TempData["ForgotEmail"] = email;
                return RedirectToAction("ForgotPassword");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["ForgotStep"] = "1";
                return RedirectToAction("ForgotPassword");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                TempData["ForgotError"] = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["ForgotStep"] = "3";
                TempData["ForgotEmail"] = email;
                return RedirectToAction("ForgotPassword");
            }

            HttpContext.Session.Remove("ForgotPasswordVerified");
            TempData["ForgotSuccess"] = _localizer["Message_PasswordResetSuccess"].Value;
            return RedirectToAction("Login");
        }
    }
}