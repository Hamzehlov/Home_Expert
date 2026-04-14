using Home_Expert.Models;
using Home_Expert.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Controllers
{   
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public SettingsController(UserManager<ApplicationUser> userManager)
            => _userManager = userManager;

        public IActionResult Settings() => View();
    }
}

namespace Home_Expert.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/Account")]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public AccountApiController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        // ──────────────────────────────────────────────────────
        // GET /api/Account/Profile
        // Returns text data only — NO binary/base64
        // ──────────────────────────────────────────────────────
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var isVendor = roles.Contains("Vendor");

            object? vendorData = null;

            if (isVendor)
            {
                var vendor = await _db.Vendors
                    .AsNoTracking()
                    .Include(v => v.ServiceType)
                    .Select(v => new
                    {
                        v.Id,
                        v.UserId,
                        CompanyNameAr = (string?)v.CompanyNameAr ?? "",
                        CompanyNameEn = (string?)v.CompanyNameEn ?? "",
                        DescriptionAr = (string?)v.DescriptionAr ?? "",
                        DescriptionEn = (string?)v.DescriptionEn ?? "",
                        ShowroomAddressAr = (string?)v.ShowroomAddressAr ?? "",
                        ShowroomAddressEn = (string?)v.ShowroomAddressEn ?? "",
                        v.YearsExperience,
                        v.Verified,
                        // flags only — no binary data
                        HasLogo = v.Logo != null,
                        HasShowroomImage = v.ShowroomImage != null,
                        HasCommercialReg = v.CommercialRegistrationFile != null,
                        HasWorkLicense = v.WorkLicenseFile != null,
                        ServiceTypeAr = (string?)v.ServiceType!.DescCodeAr ?? "",
                        ServiceTypeEn = (string?)v.ServiceType!.DescCodeEn ?? "",
                    })
                    .FirstOrDefaultAsync(v => v.UserId == user.Id);

                if (vendor != null)
                {
                    var isAr = System.Globalization.CultureInfo.CurrentUICulture.Name.StartsWith("ar");
                    vendorData = new
                    {
                        vendorId = vendor.Id,
                        companyNameAr = vendor.CompanyNameAr,
                        companyNameEn = vendor.CompanyNameEn,
                        descriptionAr = vendor.DescriptionAr,
                        descriptionEn = vendor.DescriptionEn,
                        showroomAddressAr = vendor.ShowroomAddressAr,
                        showroomAddressEn = vendor.ShowroomAddressEn,
                        yearsExperience = vendor.YearsExperience,
                        verified = vendor.Verified,
                        serviceType = isAr ? vendor.ServiceTypeAr : vendor.ServiceTypeEn,
                        // image flags — frontend uses these to decide whether to show img tags
                        hasLogo = vendor.HasLogo,
                        hasShowroomImage = vendor.HasShowroomImage,
                        hasCommercialReg = vendor.HasCommercialReg,
                        hasWorkLicense = vendor.HasWorkLicense,
                    };
                }
            }

            return Ok(new
            {
                firstNameAr = user.FirstNameAr ?? "",
                firstNameEn = user.FirstNameEn ?? "",
                lastName = user.LastName ?? "",
                email = user.Email ?? "",
                phone = user.Phone ?? user.PhoneNumber ?? "",
                role = roles.FirstOrDefault() ?? "",
                roles,
                isVendor,
                vendor = vendorData
            });
        }

    
        [HttpGet("VendorImage/{type}")]
        public async Task<IActionResult> VendorImage(string type)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            if (vendor == null) return NotFound();

            byte[]? data = type.ToLower() switch
            {
                "logo" => vendor.Logo,
                "showroom" => vendor.ShowroomImage,
                "commercial" => vendor.CommercialRegistrationFile,
                "license" => vendor.WorkLicenseFile,
                _ => null
            };

            if (data == null || data.Length == 0) return NotFound();

            // PDF files
            if (type is "commercial" or "license")
            {
                // detect PDF by magic bytes
                bool isPdf = data.Length > 4
                    && data[0] == 0x25 && data[1] == 0x50
                    && data[2] == 0x44 && data[3] == 0x46;

                return isPdf
                    ? File(data, "application/pdf")
                    : File(data, "image/jpeg");
            }

            return File(data, "image/jpeg");
        }

        // ──────────────────────────────────────────────────────
        // POST /api/Account/UpdatePhone
        // ──────────────────────────────────────────────────────
        [HttpPost("UpdatePhone")]
        public async Task<IActionResult> UpdatePhone([FromBody] ChangePasswordRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Phone) ||
                !System.Text.RegularExpressions.Regex.IsMatch(req.Phone, @"^\d{10}$"))
                return BadRequest(new { success = false, message = "رقم الهاتف غير صالح" });

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            user.Phone = req.Phone;
            user.PhoneNumber = req.Phone;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded
                ? Ok(new { success = true, message = "تم تحديث رقم الهاتف بنجاح" })
                : BadRequest(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });
        }

        // ──────────────────────────────────────────────────────
        // POST /api/Account/ChangePassword
        // ──────────────────────────────────────────────────────
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.CurrentPassword) ||
                string.IsNullOrWhiteSpace(req.NewPassword) ||
                req.NewPassword != req.ConfirmPassword ||
                req.NewPassword.Length < 8)
                return BadRequest(new { success = false, message = "البيانات غير صالحة" });

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await _userManager.ChangePasswordAsync(user, req.CurrentPassword, req.NewPassword);
            if (result.Succeeded)
                return Ok(new { success = true, message = "تم تغيير كلمة المرور بنجاح" });

            var err = result.Errors.FirstOrDefault();
            var msg = err?.Code == "PasswordMismatch"
                ? "كلمة المرور الحالية غير صحيحة"
                : string.Join(", ", result.Errors.Select(e => e.Description));

            return BadRequest(new { success = false, message = msg });
        }
    }


}