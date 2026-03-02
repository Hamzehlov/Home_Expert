using Home_Expert.Models;
using Home_Expert.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Home_Expert.Controllers
{
    public class ServiceController : Controller
    {

        private readonly ApplicationDbContext _context;
     

        public ServiceController(
            ApplicationDbContext context
            )
        {
            _context = context;
            
        }
        public async Task<IActionResult> Index()
        {
            var ads = await _context.Ads.OrderByDescending(a => a.CreatedAt).ToListAsync();
            return View(ads);
        }

        // صفحة إضافة إعلان
        public IActionResult Create()
        {
            return View();
        }

        // حفظ الإعلان الجديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ad ad, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    using var ms = new MemoryStream();
                    await ImageFile.CopyToAsync(ms);
                    ad.Image = ms.ToArray();
                }

                ad.CreatedAt = DateTime.Now;
                _context.Add(ad);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم إضافة الإعلان بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(ad);
        }

        // تعديل الإعلان
        public async Task<IActionResult> Edit(int id)
        {
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null) return NotFound();
            return View(ad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ad ad, IFormFile? ImageFile)
        {
            if (id != ad.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null)
                    {
                        using var ms = new MemoryStream();
                        await ImageFile.CopyToAsync(ms);
                        ad.Image = ms.ToArray();
                    }
                    _context.Update(ad);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "تم تعديل الإعلان بنجاح";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdExists(ad.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ad);
        }

        // حذف الإعلان
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null) return NotFound();
            _context.Ads.Remove(ad);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تم حذف الإعلان بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // تفعيل / إلغاء تفعيل الإعلان
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null) return NotFound();
            ad.IsActive = !ad.IsActive;
            _context.Update(ad);
            await _context.SaveChangesAsync();
            TempData["Success"] = ad.IsActive == true ? "تم تفعيل الإعلان" : "تم إلغاء تفعيل الإعلان";
            return RedirectToAction(nameof(Index));
        }

        private bool AdExists(int id) => _context.Ads.Any(e => e.Id == id);
    }
}
