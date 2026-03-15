using Home_Expert.Models;
using Home_Expert.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var allUsers = await _userManager.Users
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            var viewModel = new UsersIndexViewModel();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var nameAr = $"{user.FirstNameAr ?? ""} {user.LastName ?? ""}".Trim();
                var nameEn = $"{user.FirstNameEn ?? ""} {user.LastName ?? ""}".Trim();
                if (string.IsNullOrWhiteSpace(nameAr)) nameAr = user.Email ?? user.Id;
                if (string.IsNullOrWhiteSpace(nameEn)) nameEn = user.Email ?? user.Id;

                var dto = new UserDto
                {
                    Id = user.Id,
                    NameAr = nameAr,
                    NameEn = nameEn,
                    Email = user.Email ?? "—",
                    Phone = user.Phone ?? user.PhoneNumber ?? "—",
                    IsActive = user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow,
                    Roles = roles.ToList()
                };

                if (roles.Contains("Admin")) viewModel.Admins.Add(dto);
                else if (roles.Contains("Vendor")) viewModel.Vendors.Add(dto);
                else viewModel.Customers.Add(dto);
            }

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Json(new { success = false, message = "المستخدم غير موجود" });

            bool isLocked = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow;
            if (isLocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
                await _userManager.ResetAccessFailedCountAsync(user);
                return Json(new { success = true, isActive = true, message = "تم تفعيل المستخدم" });
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                return Json(new { success = true, isActive = false, message = "تم إيقاف المستخدم" });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Json(new { success = false, message = "المستخدم غير موجود" });

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded
                ? Json(new { success = true, message = "تم حذف المستخدم بنجاح" })
                : Json(new { success = false, message = "حدث خطأ أثناء الحذف" });
        }
    }

   
}