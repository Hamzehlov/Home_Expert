using Home_Expert.Models;
using Home_Expert.Services;
using Home_Expert.ViewModel.Auth;
using Home_Expert.ViewModel.RegisterVendorDto;
using Home_Expert.ViewModels.Response;
using Home_Expert.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace Home_Expert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOtpService _otpService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VendorAuthController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public VendorAuthController(
            UserManager<ApplicationUser> userManager,
            IOtpService otpService,
            ApplicationDbContext context,
            ILogger<VendorAuthController> logger,
            IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _otpService = otpService;
            _context = context;
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet("service-types")]
        public IActionResult GetServiceTypes()
        {
            var serviceTypes = _context.Codes
                .Where(x => x.ParentId == 7 && x.IsActive == true)
                .Select(s => new { s.Id, s.DescCodeEn, s.DescCodeAr })
                .ToList();

            return Ok(serviceTypes);
        }

        /// <summary>
        /// الخطوة 1: التسجيل وتخزين البيانات مؤقتًا وإرسال OTP
        /// POST: /api/vendorauth/register
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterVendorViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = _localizer["Message_InvalidData"].Value,
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                    });
                }

                // ✅ تحقق من وجود مستخدم بنفس البريد
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    // شيك على الـ Role
                    var roles = await _userManager.GetRolesAsync(existingUser);

                    if (roles.Contains("User"))
                    {
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = _localizer["Message_EmailExistsAsCustomer"].Value
                        });
                    }
                    else if (roles.Contains("Vendor"))
                    {
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = _localizer["Message_EmailExistsAsVendor"].Value
                        });
                    }
                    else if (roles.Contains("Admin"))
                    {
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = _localizer["Message_EmailExistsAsAdmin"].Value
                        });
                    }
                    else
                    {
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = _localizer["Message_EmailExists"].Value
                        });
                    }
                }

                // توليد OTP
                string otp = _otpService.GenerateOtp();
                DateTime otpExpiry = DateTime.UtcNow.AddMinutes(5);

                // تحويل Logo إلى Base64
                string? logoBase64 = null;
                if (model.Logo != null && model.Logo.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await model.Logo.CopyToAsync(ms);
                    logoBase64 = Convert.ToBase64String(ms.ToArray());
                }

                // تحويل ShowroomImage إلى Base64
                string? showroomImageBase64 = null;
                if (model.ShowroomImage != null && model.ShowroomImage.Length > 0)
                {
                    using var ms2 = new MemoryStream();
                    await model.ShowroomImage.CopyToAsync(ms2);
                    showroomImageBase64 = Convert.ToBase64String(ms2.ToArray());
                }

                // تحويل CommercialRegistrationFile إلى Base64
                string? commercialFileBase64 = null;
                if (model.CommercialRegistrationFile != null && model.CommercialRegistrationFile.Length > 0)
                {
                    using var ms3 = new MemoryStream();
                    await model.CommercialRegistrationFile.CopyToAsync(ms3);
                    commercialFileBase64 = Convert.ToBase64String(ms3.ToArray());
                }

                // تحويل WorkLicenseFile إلى Base64
                string? workLicenseBase64 = null;
                if (model.WorkLicenseFile != null && model.WorkLicenseFile.Length > 0)
                {
                    using var ms4 = new MemoryStream();
                    await model.WorkLicenseFile.CopyToAsync(ms4);
                    workLicenseBase64 = Convert.ToBase64String(ms4.ToArray());
                }

                // تخزين كل البيانات مؤقتًا في Session
                var sessionData = new
                {
                    model.FirstNameAr,
                    model.FirstNameEn,
                    model.Email,
                    model.Password,
                    model.Phone,
                    model.CompanyNameAr,
                    model.CompanyNameEn,
                    model.DescriptionAr,
                    model.DescriptionEn,
                    model.YearsExperience,
                    model.ServiceTypeId,
                    Logo = logoBase64,
                    model.ShowroomAddressAr,
                    model.ShowroomAddressEn,
                    ShowroomImage = showroomImageBase64,
                    CommercialRegistrationFile = commercialFileBase64,
                    WorkLicenseFile = workLicenseBase64,
                    OtpCode = otp,
                    OtpExpiry = otpExpiry,
                    OtpAttempts = 0
                };
                HttpContext.Session.SetString("VendorRegisterData", JsonSerializer.Serialize(sessionData));

                // إرسال OTP
                await _otpService.SendOtpEmail(model.Email, otp);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = _localizer["Message_OTPSent"].Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التسجيل");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = _localizer["Message_ServerError"].Value
                });
            }
        }

        /// <summary>
        /// الخطوة 2: التحقق من OTP وإنشاء المستخدم بعد النجاح
        /// POST: /api/vendorauth/verify-otp
        /// </summary>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = _localizer["Message_InvalidData"].Value,
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                    });
                }

                var sessionDataStr = HttpContext.Session.GetString("VendorRegisterData");
                if (string.IsNullOrEmpty(sessionDataStr))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = _localizer["Message_SessionExpired"].Value
                    });
                }

                var sessionData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(sessionDataStr)!;

                string storedOtp = sessionData["OtpCode"].GetString()!;
                DateTime otpExpiry = sessionData["OtpExpiry"].GetDateTime();
                int otpAttempts = sessionData["OtpAttempts"].GetInt32();

                // ✅ تحقق من OTP أولاً
                bool isOtpValid = _otpService.ValidateOtp(model.OtpCode.Trim(), storedOtp.Trim(), otpExpiry);

                if (!isOtpValid)
                {
                    otpAttempts++;
                    if (otpAttempts >= 3)
                    {
                        HttpContext.Session.Remove("VendorRegisterData");
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = _localizer["Message_MaxAttemptsReached"].Value
                        });
                    }

                    sessionData["OtpAttempts"] = JsonSerializer.SerializeToElement(otpAttempts);
                    HttpContext.Session.SetString("VendorRegisterData", JsonSerializer.Serialize(sessionData));

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = string.Format(_localizer["Message_OTPIncorrect"].Value, 3 - otpAttempts)
                    });
                }

                // إنشاء المستخدم
                var firstNameAr = sessionData["FirstNameAr"].GetString();
                var firstNameEn = sessionData["FirstNameEn"].GetString();

                var user = new ApplicationUser
                {
                    UserName = sessionData["Email"].GetString()!,
                    Email = sessionData["Email"].GetString()!,
                    FirstNameAr = firstNameAr!,
                    FirstNameEn = firstNameEn!,
                    LastName = firstNameEn,
                    Phone = sessionData.TryGetValue("Phone", out var phone) ? phone.GetString() : null,
                    EmailConfirmed = true,
                    EmailVerifiedAt = DateTime.UtcNow,
                    OTPCode = storedOtp,
                    OTPExpiry = otpExpiry,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                string password = sessionData["Password"].GetString()!;
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = _localizer["Message_UserCreationFailed"].Value,
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    });
                }

                await _userManager.AddToRoleAsync(user, "Vendor");

                // استرجاع الملفات من الـ Session
                byte[]? logoBytes = null;
                if (sessionData.TryGetValue("Logo", out var logoElement) && !string.IsNullOrEmpty(logoElement.GetString()))
                {
                    logoBytes = Convert.FromBase64String(logoElement.GetString()!);
                }

                byte[]? showroomImageBytes = null;
                if (sessionData.TryGetValue("ShowroomImage", out var siEl) && !string.IsNullOrEmpty(siEl.GetString()))
                {
                    showroomImageBytes = Convert.FromBase64String(siEl.GetString()!);
                }

                byte[]? commercialFileBytes = null;
                if (sessionData.TryGetValue("CommercialRegistrationFile", out var crEl) && !string.IsNullOrEmpty(crEl.GetString()))
                {
                    commercialFileBytes = Convert.FromBase64String(crEl.GetString()!);
                }

                byte[]? workLicenseBytes = null;
                if (sessionData.TryGetValue("WorkLicenseFile", out var wlEl) && !string.IsNullOrEmpty(wlEl.GetString()))
                {
                    workLicenseBytes = Convert.FromBase64String(wlEl.GetString()!);
                }

                // استرجاع العناوين
                string? showroomAddrAr = sessionData.TryGetValue("ShowroomAddressAr", out var addrAr) ? addrAr.GetString() : null;
                string? showroomAddrEn = sessionData.TryGetValue("ShowroomAddressEn", out var addrEn) ? addrEn.GetString() : null;

                // استرجاع الأوصاف
                string? descAr = sessionData.TryGetValue("DescriptionAr", out var dAr) ? dAr.GetString() : null;
                string? descEn = sessionData.TryGetValue("DescriptionEn", out var dEn) ? dEn.GetString() : null;

                // استرجاع أسماء الشركة
                string? companyNameAr = sessionData["CompanyNameAr"].GetString();
                string? companyNameEn = sessionData["CompanyNameEn"].GetString();

                var vendor = new Vendor
                {
                    UserId = user.Id,
                    CompanyNameAr = companyNameAr!,
                    CompanyNameEn = companyNameEn!,
                    DescriptionAr = descAr,
                    DescriptionEn = descEn,
                    YearsExperience = sessionData.TryGetValue("YearsExperience", out var years) ? years.GetInt32() : 0,
                    ServiceTypeId = 7,
                    Logo = logoBytes,
                    Verified = false,
                    RatingAvg = 0,
                    CompletedOrders = 0,
                    ShowroomAddressAr = showroomAddrAr,
                    ShowroomAddressEn = showroomAddrEn,
                    ShowroomImage = showroomImageBytes,
                    CommercialRegistrationFile = commercialFileBytes,
                    WorkLicenseFile = workLicenseBytes
                };

                _context.Vendors.Add(vendor);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("VendorRegisterData");

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = _localizer["Message_RegistrationSuccess"].Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من OTP");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = _localizer["Message_ServerError"].Value
                });
            }
        }

        /// <summary>
        /// إعادة إرسال OTP
        /// POST: /api/vendorauth/resend-otp
        /// </summary>
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = _localizer["Message_InvalidData"].Value,
                        Errors = errors
                    });
                }

                var sessionData = HttpContext.Session.GetString("VendorRegisterData");

                if (string.IsNullOrEmpty(sessionData))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = _localizer["Message_SessionExpired"].Value
                    });
                }

                var registerData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(sessionData);

                // التحقق من أن البريد مطابق
                string sessionEmail = registerData["Email"].GetString();
                if (sessionEmail != model.Email)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = _localizer["Message_InvalidData"].Value
                    });
                }

                // توليد OTP جديد
                string newOtp = _otpService.GenerateOtp();
                DateTime newExpiry = DateTime.UtcNow.AddMinutes(5);

                registerData["OtpCode"] = JsonSerializer.SerializeToElement(newOtp);
                registerData["OtpExpiry"] = JsonSerializer.SerializeToElement(newExpiry);
                registerData["OtpAttempts"] = JsonSerializer.SerializeToElement(0);

                HttpContext.Session.SetString("VendorRegisterData",
                    JsonSerializer.Serialize(registerData));

                // إرسال OTP الجديد
                bool emailSent = await _otpService.SendOtpEmail(model.Email, newOtp);

                if (!emailSent)
                {
                    _logger.LogWarning("Email sending failed, but OTP was generated: {Otp}", newOtp);
                }

                var response = new RegisterResponseViewModel
                {
                    Email = model.Email,
                    OtpExpiresAt = newExpiry,
                    Message = _localizer["Message_OTPResent"].Value
                };

                return Ok(new ApiResponse<RegisterResponseViewModel>
                {
                    Success = true,
                    Message = _localizer["Message_OTPResent"].Value,
                    Data = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إعادة إرسال OTP");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = _localizer["Message_ServerError"].Value
                });
            }
        }
    }
}
