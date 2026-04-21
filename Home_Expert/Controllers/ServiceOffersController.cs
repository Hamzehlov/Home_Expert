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

        // أرقام أنواع الخدمات من جدول Code
        private const int SERVICE_TYPE_KITCHEN_SALE = 29;        // بيع مطابخ
        private const int SERVICE_TYPE_FURNITURE_MOVING = 30;    // نقل أثاث

        // أرقام الحالات
        private const int REQUEST_ACCEPTED = 81;
        private const int REQUEST_REJECTED = 82;
        private const int OFFER_PENDING = 86;
        private const int OFFER_SUBMITTED = 87;
        private const int OFFER_ACCEPTED = 88;
        private const int OFFER_REJECTED = 89;

        public ServiceOffersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ══════════════════════════════════════════════════════
        // GET: /ServiceOffers/Index
        // عرض كل الطلبات المرتبطة بهذه الشركة
        // ══════════════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            if (vendor == null) return RedirectToAction("Index", "Home");

            // ✅ جلب جميع عروض الشركة بدون فلترة حسب الخدمات
            var offers = await _db.ServiceOffers
                .AsNoTracking()
                .Where(o => o.VendorId == vendor.Id)  // فقط شرط VendorId
                .Include(o => o.Status)
                .Include(o => o.Request)
                    .ThenInclude(r => r.Status)
                .Include(o => o.Request)
                    .ThenInclude(r => r.Service)
                        .ThenInclude(s => s.Type)
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
                    RequestStatusAr = o.Request.Status.DescCodeAr,
                    RequestStatusEn = o.Request.Status.DescCodeEn,
                    CustomerNameAr = (o.Request.Customer.FirstNameAr ?? "") + " " + (o.Request.Customer.LastName ?? ""),
                    CustomerNameEn = (o.Request.Customer.FirstNameEn ?? "") + " " + (o.Request.Customer.LastName ?? ""),
                })
                .ToListAsync();

            foreach (var offer in offers)
            {
                offer.CanEditOffer = offer.OfferStatusId != OFFER_ACCEPTED &&
                                     offer.OfferStatusId != OFFER_REJECTED &&
                                     offer.RequestStatusId != REQUEST_ACCEPTED &&
                                     offer.RequestStatusId != REQUEST_REJECTED;
            }

            ViewBag.TotalOffers = offers.Count;
            ViewBag.PendingOffers = offers.Count(o => o.OfferStatusId == OFFER_PENDING);
            ViewBag.SubmittedOffers = offers.Count(o => o.OfferStatusId == OFFER_SUBMITTED);
            ViewBag.AcceptedOffers = offers.Count(o => o.OfferStatusId == OFFER_ACCEPTED);
            ViewBag.RejectedOffers = offers.Count(o => o.OfferStatusId == OFFER_REJECTED);

            return View(offers);
        }
        // ══════════════════════════════════════════════════════
        // GET: /ServiceOffers/Details/5
        // تفاصيل الطلب والخدمة مع إمكانية رفع المرفقات
        // ══════════════════════════════════════════════════════
        // في ServiceOffersController.cs - دالة Details

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

            // ✅ فقط الرقم - لا تحقق على النص
            int? serviceTypeId = offer.Request.Service.TypeId;

            // أرقام أنواع الخدمات من جدول Code
            const int SERVICE_TYPE_KITCHEN_SALE = 29;        // بيع مطابخ
            const int SERVICE_TYPE_FURNITURE_MOVING = 30;    // نقل أثاث

            // ✅ التحقق فقط على الرقم
            bool isKitchenService = (serviceTypeId == SERVICE_TYPE_KITCHEN_SALE);
            bool isMovingService = (serviceTypeId == SERVICE_TYPE_FURNITURE_MOVING);

            // جلب بيانات المطابخ
            KitchenMeasurement? kitchenMeasurement = null;
            KitchenCostEstimate? kitchenCostEstimate = null;
            List<KitchenExport>? kitchenExports = null;

            if (isKitchenService)
            {
                kitchenMeasurement = await _db.KitchenMeasurements
                    .AsNoTracking()
                    .Include(km => km.InputMode)
                    .Include(km => km.Unit)
                    .FirstOrDefaultAsync(km => km.ServiceRequestId == offer.RequestId);

                if (kitchenMeasurement != null)
                {
                    kitchenCostEstimate = await _db.KitchenCostEstimates
                        .AsNoTracking()
                        .Include(kce => kce.WoodType)
                        .Include(kce => kce.Status)
                        .FirstOrDefaultAsync(kce => kce.MeasurementId == kitchenMeasurement.Id);

                    kitchenExports = await _db.KitchenExports
                        .AsNoTracking()
                        .Include(ke => ke.Format)
                        .Where(ke => ke.MeasurementId == kitchenMeasurement.Id)
                        .ToListAsync();
                }
            }

            // جلب بيانات النقل إذا كانت الخدمة نقل أثاث
            MovingRequest? movingRequest = null;
            List<MovingOffer>? movingOffers = null;
            List<MovingStatusLog>? movingStatusLogs = null;

            if (isMovingService)
            {
                // ✅ استرجاع طلب النقل المرتبط بطلب الخدمة
                movingRequest = await _db.MovingRequests
                    .AsNoTracking()
                    .Include(mr => mr.Status)
                    .FirstOrDefaultAsync(mr => mr.ServiceRequestId == offer.RequestId);

                if (movingRequest != null)
                {
                    // ✅ استرجاع عروض النقل لهذا الطلب
                    movingOffers = await _db.MovingOffers
                        .AsNoTracking()
                        .Include(mo => mo.Status)
                        .Where(mo => mo.MovingRequestId == movingRequest.Id && mo.VendorId == vendor.Id)
                        .ToListAsync();

                    // ✅ استرجاع سجل الحالة
                    movingStatusLogs = await _db.MovingStatusLogs
                        .AsNoTracking()
                        .Include(msl => msl.Status)
                        .Where(msl => msl.MovingRequestId == movingRequest.Id)
                        .OrderByDescending(msl => msl.Id)
                        .ToListAsync();
                }
            }

            // أرقام الحالات
            const int REQUEST_ACCEPTED = 81;
            const int REQUEST_REJECTED = 82;
            const int OFFER_PENDING = 86;
            const int OFFER_SUBMITTED = 87;
            const int OFFER_ACCEPTED = 88;
            const int OFFER_REJECTED = 89;

            bool canSubmit = offer.StatusId != OFFER_ACCEPTED &&
                             offer.StatusId != OFFER_REJECTED &&
                             offer.Request.StatusId != REQUEST_ACCEPTED &&
                             offer.Request.StatusId != REQUEST_REJECTED;

            string statusKey = offer.StatusId switch
            {
                OFFER_PENDING => "pending",
                OFFER_SUBMITTED => "submitted",
                OFFER_ACCEPTED => "accepted",
                OFFER_REJECTED => "rejected",
                _ => "pending"
            };

            var vm = new ServiceOfferDetailsVm
            {
                OfferId = offer.Id,
                RequestId = offer.RequestId,
                PriceEstimate = offer.PriceEstimate,
                InspectionRequired = offer.InspectionRequired ?? false,
                OfferStatusId = offer.StatusId,
                OfferStatusAr = offer.Status.DescCodeAr,
                OfferStatusEn = offer.Status.DescCodeEn,
                StatusKey = statusKey,
                CanSubmit = canSubmit,

                ServiceNameAr = offer.Request.Service.NameAr,
                ServiceNameEn = offer.Request.Service.NameEn,
                ServiceTypeAr = offer.Request.Service.Type?.DescCodeAr ?? "",
                ServiceTypeEn = offer.Request.Service.Type?.DescCodeEn ?? "",
                ServiceCatAr = offer.Request.Service.Category?.DescCodeAr ?? "",
                ServiceCatEn = offer.Request.Service.Category?.DescCodeEn ?? "",

                PreferredTime = offer.Request.PreferredTime,
                RequestStatusId = offer.Request.StatusId,
                RequestStatusAr = offer.Request.Status.DescCodeAr,
                RequestStatusEn = offer.Request.Status.DescCodeEn,

                CustomerNameAr = ((offer.Request.Customer.FirstNameAr ?? "") + " " + (offer.Request.Customer.LastName ?? "")).Trim(),
                CustomerNameEn = ((offer.Request.Customer.FirstNameEn ?? "") + " " + (offer.Request.Customer.LastName ?? "")).Trim(),
                CustomerEmail = offer.Request.Customer.Email ?? "",
                CustomerPhone = offer.Request.Customer.Phone ?? offer.Request.Customer.PhoneNumber ?? "",
                CustomerCity = offer.Request.Customer.UserLocations.FirstOrDefault()?.City ?? "",
                CustomerArea = offer.Request.Customer.UserLocations.FirstOrDefault()?.Area ?? "",
                CustomerLatitude = offer.Request.Customer.UserLocations.FirstOrDefault()?.Latitude,
                CustomerLongitude = offer.Request.Customer.UserLocations.FirstOrDefault()?.Longitude,

                HasAttachment = offer.PdfFile != null && offer.PdfFile.Length > 0,
                AttachmentFileName = offer.PdfFile != null ? $"Attachment_{offer.Id}" : null,

                IsKitchenService = isKitchenService,
                KitchenMeasurement = kitchenMeasurement,
                KitchenCostEstimate = kitchenCostEstimate,
                KitchenExports = kitchenExports,

                IsMovingService = isMovingService,
                MovingRequest = movingRequest,
                MovingOffers = movingOffers,
                MovingStatusLogs = movingStatusLogs
            };

            return View(vm);
        }
        // ══════════════════════════════════════════════════════
        // POST: /ServiceOffers/SubmitOffer
        // تقديم أو تعديل عرض السعر مع إمكانية رفع ملف مرفق
        // ══════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOffer([FromForm] SubmitOfferRequest req, IFormFile? attachmentFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.UserId == user.Id);
            if (vendor == null)
                return Json(new { success = false, message = "الشركة غير موجودة" });

            var offer = await _db.ServiceOffers
                .Include(o => o.Request)
                .FirstOrDefaultAsync(o => o.Id == req.OfferId && o.VendorId == vendor.Id);

            if (offer == null)
                return Json(new { success = false, message = "العرض غير موجود" });

            // منع التعديل إذا كان العرض مقبولاً أو مرفوضاً
            if (offer.StatusId == OFFER_ACCEPTED || offer.StatusId == OFFER_REJECTED)
                return Json(new { success = false, message = "لا يمكن تعديل هذا العرض لأنه مقبول أو مرفوض" });

            // منع التعديل إذا كانت حالة الطلب مقبول أو مرفوض
            if (offer.Request.StatusId == REQUEST_ACCEPTED)
                return Json(new { success = false, message = "لا يمكن تعديل العرض بعد قبول الطلب" });

            if (offer.Request.StatusId == REQUEST_REJECTED)
                return Json(new { success = false, message = "لا يمكن تعديل العرض بعد رفض الطلب" });

            // تحديث البيانات الأساسية
            offer.PriceEstimate = req.PriceEstimate;
            offer.InspectionRequired = req.InspectionRequired;

            // فقط إذا كان العرض لا يزال في حالة Pending نقوم بتغييره إلى Submitted
            if (offer.StatusId == OFFER_PENDING)
                offer.StatusId = OFFER_SUBMITTED;

            // معالجة رفع الملف
            if (attachmentFile != null && attachmentFile.Length > 0)
            {
                // التحقق من حجم الملف (حد أقصى 5 ميجابايت)
                if (attachmentFile.Length > 5 * 1024 * 1024)
                    return Json(new { success = false, message = "حجم الملف يتجاوز 5 ميجابايت" });

                // التحقق من امتداد الملف
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(attachmentFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                    return Json(new { success = false, message = "نوع الملف غير مسموح. المسموح: PDF, Word, صور" });

                using var memoryStream = new MemoryStream();
                await attachmentFile.CopyToAsync(memoryStream);
                offer.PdfFile = memoryStream.ToArray();
            }

            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "تم تقديم العرض بنجاح" });
        }

        // ══════════════════════════════════════════════════════
        // GET: /ServiceOffers/DownloadAttachment
        // تحميل الملف المرفق
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> DownloadAttachment(int offerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.UserId == user.Id);
            if (vendor == null) return NotFound();

            var offer = await _db.ServiceOffers
                .Where(o => o.Id == offerId && o.VendorId == vendor.Id)
                .Select(o => new { o.PdfFile })
                .FirstOrDefaultAsync();

            if (offer == null || offer.PdfFile == null || offer.PdfFile.Length == 0)
                return NotFound();

            // تحديد نوع الملف من التواقيع (magic bytes)
            string contentType = "application/octet-stream";
            string fileExtension = ".bin";

            if (offer.PdfFile.Length > 4)
            {
                // PDF
                if (offer.PdfFile[0] == 0x25 && offer.PdfFile[1] == 0x50 &&
                    offer.PdfFile[2] == 0x44 && offer.PdfFile[3] == 0x46)
                {
                    contentType = "application/pdf";
                    fileExtension = ".pdf";
                }
                // JPEG
                else if (offer.PdfFile[0] == 0xFF && offer.PdfFile[1] == 0xD8)
                {
                    contentType = "image/jpeg";
                    fileExtension = ".jpg";
                }
                // PNG
                else if (offer.PdfFile[0] == 0x89 && offer.PdfFile[1] == 0x50 &&
                         offer.PdfFile[2] == 0x4E && offer.PdfFile[3] == 0x47)
                {
                    contentType = "image/png";
                    fileExtension = ".png";
                }
                // DOC (OLE2)
                else if (offer.PdfFile[0] == 0xD0 && offer.PdfFile[1] == 0xCF &&
                         offer.PdfFile[2] == 0x11 && offer.PdfFile[3] == 0xE0)
                {
                    contentType = "application/msword";
                    fileExtension = ".doc";
                }
                // DOCX (ZIP)
                else if (offer.PdfFile[0] == 0x50 && offer.PdfFile[1] == 0x4B)
                {
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    fileExtension = ".docx";
                }
            }

            string fileName = $"Attachment_{offerId}{fileExtension}";
            return File(offer.PdfFile, contentType, fileName);
        }

        // ══════════════════════════════════════════════════════
        // GET: /ServiceOffers/DownloadKitchenPlan
        // تحميل مخطط المطبخ ثنائي الأبعاد
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> DownloadKitchenPlan(int measurementId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.UserId == user.Id);
            if (vendor == null) return NotFound();

            var measurement = await _db.KitchenMeasurements
                .Include(km => km.ServiceRequest)
                    .ThenInclude(sr => sr.ServiceOffers)
                .FirstOrDefaultAsync(km => km.Id == measurementId);

            if (measurement == null || measurement.Generated2Dplan == null)
                return NotFound();

            // التحقق من أن البائع لديه عرض لهذا الطلب
            var hasOffer = measurement.ServiceRequest.ServiceOffers.Any(o => o.VendorId == vendor.Id);
            if (!hasOffer) return Unauthorized();

            return File(measurement.Generated2Dplan, "image/png", $"Kitchen_Plan_{measurementId}.png");
        }

        // ══════════════════════════════════════════════════════
        // GET: /ServiceOffers/DownloadKitchenExport
        // تحميل ملف مطبخ مصدر (PDF, CAD, etc.)
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> DownloadKitchenExport(int exportId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.UserId == user.Id);
            if (vendor == null) return NotFound();

            var export = await _db.KitchenExports
                .Include(ke => ke.Measurement)
                    .ThenInclude(km => km.ServiceRequest)
                        .ThenInclude(sr => sr.ServiceOffers)
                .FirstOrDefaultAsync(ke => ke.Id == exportId);

            if (export == null || export.File == null)
                return NotFound();

            var hasOffer = export.Measurement.ServiceRequest.ServiceOffers.Any(o => o.VendorId == vendor.Id);
            if (!hasOffer) return Unauthorized();

            string contentType = "application/octet-stream";
            string extension = ".bin";

            // تحديد نوع الملف
            if (export.File.Length > 4)
            {
                if (export.File[0] == 0x25 && export.File[1] == 0x50 && export.File[2] == 0x44 && export.File[3] == 0x46)
                {
                    contentType = "application/pdf";
                    extension = ".pdf";
                }
                else if (export.File[0] == 0xFF && export.File[1] == 0xD8)
                {
                    contentType = "image/jpeg";
                    extension = ".jpg";
                }
                else if (export.File[0] == 0x89 && export.File[1] == 0x50 && export.File[2] == 0x4E && export.File[3] == 0x47)
                {
                    contentType = "image/png";
                    extension = ".png";
                }
            }

            string fileName = $"Kitchen_Export_{exportId}{extension}";
            return File(export.File, contentType, fileName);
        }

        // ══════════════════════════════════════════════════════
        // GET: /ServiceOffers/DownloadMovingProof
        // تحميل إثبات حالة النقل
        // ══════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> DownloadMovingProof(int logId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.UserId == user.Id);
            if (vendor == null) return NotFound();

            var log = await _db.MovingStatusLogs
                .Include(msl => msl.MovingRequest)
                    .ThenInclude(mr => mr.ServiceRequest)
                        .ThenInclude(sr => sr.ServiceOffers)
                .FirstOrDefaultAsync(msl => msl.Id == logId);

            if (log == null || log.Proof == null)
                return NotFound();

            var hasOffer = log.MovingRequest.ServiceRequest.ServiceOffers.Any(o => o.VendorId == vendor.Id);
            if (!hasOffer) return Unauthorized();

            string contentType = "application/octet-stream";
            string extension = ".bin";

            if (log.Proof.Length > 4)
            {
                if (log.Proof[0] == 0x25 && log.Proof[1] == 0x50 && log.Proof[2] == 0x44 && log.Proof[3] == 0x46)
                {
                    contentType = "application/pdf";
                    extension = ".pdf";
                }
                else if (log.Proof[0] == 0xFF && log.Proof[1] == 0xD8)
                {
                    contentType = "image/jpeg";
                    extension = ".jpg";
                }
                else if (log.Proof[0] == 0x89 && log.Proof[1] == 0x50 && log.Proof[2] == 0x4E && log.Proof[3] == 0x47)
                {
                    contentType = "image/png";
                    extension = ".png";
                }
            }

            string fileName = $"Moving_Proof_{logId}{extension}";
            return File(log.Proof, contentType, fileName);
        }
    }
}