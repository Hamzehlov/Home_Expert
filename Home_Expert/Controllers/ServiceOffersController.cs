using Home_Expert.Models;
using Home_Expert.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Controllers
{
    [Authorize(Roles = "Vendor")]
    public class ServiceOffersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceOffersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ══════════════════════════════════════════════════════
        // GET: /ServiceOffers/Index
        // عرض كل الطلبات المرتبطة بهذه الشركة
        // شرط: الخدمة ضمن VendorServices + IsActive = true
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            if (vendor == null) return RedirectToAction("Index", "Home");

            // الخدمات الفعالة التابعة لهذه الشركة
            var vendorServiceIds = await _db.VendorServices
                .AsNoTracking()
                .Where(vs => vs.VendorId == vendor.Id && vs.Service.IsActive)
                .Select(vs => vs.ServiceId)
                .ToListAsync();

            // جلب العروض المرتبطة بهذه الشركة على خدماتها الفعالة
            var offers = await _db.ServiceOffers
                .AsNoTracking()
                .Where(o => o.VendorId == vendor.Id
                         && vendorServiceIds.Contains(o.Request.ServiceId))
                .Include(o => o.Status)
                .Include(o => o.Request)
                    .ThenInclude(r => r.Service)
                        .ThenInclude(s => s.Type)
                .Include(o => o.Request)
                    .ThenInclude(r => r.Status)
                .Include(o => o.Request)
                    .ThenInclude(r => r.Customer)
                .OrderByDescending(o => o.Id)
                .Select(o => new ServiceOfferDto
                {
                    OfferId = o.Id,
                    RequestId = o.RequestId,
                    ServiceNameAr = o.Request.Service.NameAr,
                    ServiceNameEn = o.Request.Service.NameEn,
                    ServiceTypeAr = o.Request.Service.Type.DescCodeAr,
                    ServiceTypeEn = o.Request.Service.Type.DescCodeEn,
                    PreferredTime = o.Request.PreferredTime,
                    PriceEstimate = o.PriceEstimate,
                    InspectionRequired = o.InspectionRequired ?? false,
                    OfferStatusId = o.StatusId,
                    OfferStatusAr = o.Status.DescCodeAr,
                    OfferStatusEn = o.Status.DescCodeEn,
                    RequestStatusId = o.Request.StatusId,
                    CustomerNameAr = (o.Request.Customer.FirstNameAr ?? "") + " " + (o.Request.Customer.LastName ?? ""),
                    CustomerNameEn = (o.Request.Customer.FirstNameEn ?? "") + " " + (o.Request.Customer.LastName ?? ""),
                })
                .ToListAsync();

            ViewBag.TotalOffers = offers.Count;
            ViewBag.PendingOffers = offers.Count(o => o.OfferStatusId == 86);
            ViewBag.SubmittedOffers = offers.Count(o => o.OfferStatusId == 87);
            ViewBag.AcceptedOffers = offers.Count(o => o.OfferStatusId == 88);
            ViewBag.RejectedOffers = offers.Count(o => o.OfferStatusId == 89);

            return View(offers);
        }

        // ══════════════════════════════════════════════════════
        // POST: /ServiceOffers/SubmitOffer
        // الشركة تقدم عرض السعر
        // تحديث ServiceOffer فقط — StatusId من 86 → 87
        // ══════════════════════════════════════════════════════

        // ══════════════════════════════════════════════════════
        // GET: /ServiceOffers/Details/5
        // تفاصيل الطلب والخدمة
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            if (vendor == null) return RedirectToAction("Index", "Home");

            var offer = await _db.ServiceOffers
                .AsNoTracking()
                .Where(o => o.Id == id && o.VendorId == vendor.Id)
                .Include(o => o.Status)
                .Include(o => o.Request)
                    .ThenInclude(r => r.Service)
                        .ThenInclude(s => s.Type)
                .Include(o => o.Request)
                    .ThenInclude(r => r.Service)
                        .ThenInclude(s => s.Category)
                .Include(o => o.Request)
                    .ThenInclude(r => r.Status)
                .Include(o => o.Request)
                    .ThenInclude(r => r.Customer)
                        .ThenInclude(c => c.UserLocations)
                .FirstOrDefaultAsync();

            if (offer == null) return NotFound();

            var vm = new ServiceOfferDetailsVm
            {
                OfferId = offer.Id,
                RequestId = offer.RequestId,
                PriceEstimate = offer.PriceEstimate,
                InspectionRequired = offer.InspectionRequired ?? false,
                OfferStatusId = offer.StatusId,
                OfferStatusAr = offer.Status.DescCodeAr,
                OfferStatusEn = offer.Status.DescCodeEn,

                // Service
                ServiceNameAr = offer.Request.Service.NameAr,
                ServiceNameEn = offer.Request.Service.NameEn,
                ServiceTypeAr = offer.Request.Service.Type?.DescCodeAr ?? "",
                ServiceTypeEn = offer.Request.Service.Type?.DescCodeEn ?? "",
                ServiceCatAr = offer.Request.Service.Category?.DescCodeAr ?? "",
                ServiceCatEn = offer.Request.Service.Category?.DescCodeEn ?? "",

                // Request
                PreferredTime = offer.Request.PreferredTime,
                RequestStatusId = offer.Request.StatusId,
                RequestStatusAr = offer.Request.Status.DescCodeAr,
                RequestStatusEn = offer.Request.Status.DescCodeEn,

                // Customer
                CustomerNameAr = ((offer.Request.Customer.FirstNameAr ?? "") + " " + (offer.Request.Customer.LastName ?? "")).Trim(),
                CustomerNameEn = ((offer.Request.Customer.FirstNameEn ?? "") + " " + (offer.Request.Customer.LastName ?? "")).Trim(),
                CustomerEmail = offer.Request.Customer.Email ?? "",
                CustomerPhone = offer.Request.Customer.Phone ?? offer.Request.Customer.PhoneNumber ?? "",

                // Location (first location of customer)
                CustomerCity = offer.Request.Customer.UserLocations.FirstOrDefault()?.City ?? "",
                CustomerArea = offer.Request.Customer.UserLocations.FirstOrDefault()?.Area ?? "",
                CustomerLatitude = offer.Request.Customer.UserLocations.FirstOrDefault()?.Latitude,
                CustomerLongitude = offer.Request.Customer.UserLocations.FirstOrDefault()?.Longitude,
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOffer([FromBody] SubmitOfferRequest req)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            if (vendor == null)
                return Json(new { success = false, message = "الشركة غير موجودة" });

            // تأكد إن العرض تابع لهذه الشركة
            var offer = await _db.ServiceOffers
                .FirstOrDefaultAsync(o => o.Id == req.OfferId && o.VendorId == vendor.Id);

            if (offer == null)
                return Json(new { success = false, message = "العرض غير موجود" });

            // لا تسمح بتعديل عرض مقبول أو مرفوض
            if (offer.StatusId == 88 || offer.StatusId == 89)
                return Json(new { success = false, message = "لا يمكن تعديل هذا العرض" });

            // تحديث ServiceOffer فقط
            offer.PriceEstimate = req.PriceEstimate;
            offer.InspectionRequired = req.InspectionRequired;
            offer.StatusId = 87; // Submitted

            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "تم تقديم العرض بنجاح" });
        }
    }



 
}