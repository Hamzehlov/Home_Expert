using Home_Expert.Models;
using Home_Expert.Resources;
using Home_Expert.ViewModel.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
namespace Home_Expert.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<RegistrationController> _logger;
        private readonly ApplicationDbContext _context; 

        public RegistrationController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<SharedResource> localizer,
            ILogger<RegistrationController> logger,
            ApplicationDbContext context) 
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _localizer = localizer;
            _logger = logger;
            _context = context; 
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
                if (userRoles.Contains("Customer"))
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

                    // ✅ إذا Verified = false أو null
                    if (vendor != null && vendor.Verified != true)
                    {
                        TempData["PendingEmail"] = model.Email;
                        return RedirectToAction("PendingVerification", "Registration");
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
    }
}