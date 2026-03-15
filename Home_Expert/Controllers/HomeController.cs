using Home_Expert.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Home_Expert.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db)
        {
            _logger = logger;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                // ── Vendor Application counts ──
                var vendors = await _db.Vendors.AsNoTracking().ToListAsync();
                ViewBag.PendingCount = vendors.Count(v => v.Verified == 0);
                ViewBag.ApprovedCount = vendors.Count(v => v.Verified == 1);
                ViewBag.RejectedCount = vendors.Count(v => v.Verified == 2);

                // ── User counts ──
                var userIds = await _userManager.Users.AsNoTracking().Select(u => u.Id).ToListAsync();
                var adminIds = (await _userManager.GetUsersInRoleAsync("Admin")).Select(u => u.Id).ToHashSet();
                var vendorIds = (await _userManager.GetUsersInRoleAsync("Vendor")).Select(u => u.Id).ToHashSet();

                ViewBag.TotalUsers = userIds.Count;
                ViewBag.TotalVendors = vendorIds.Count;
                ViewBag.TotalCustomers = userIds.Count(id => !adminIds.Contains(id) && !vendorIds.Contains(id));
            }

            return View();
        }
        public IActionResult create()
        {
            return View();
        }
        public IActionResult edit()
        {
            return View();
        }
        public IActionResult details() 
            {
                return View();
            }
        public IActionResult table()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
