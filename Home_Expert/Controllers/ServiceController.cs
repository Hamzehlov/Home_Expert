using Home_Expert.Models;
using Home_Expert.Resources;
using Home_Expert.ViewModel.Service;
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
            // جلب كل الإعلانات مرتبة حسب تاريخ الإنشاء
            var ads = await _context.Ads
                                    .OrderByDescending(a => a.CreatedAt)
                                    .ToListAsync();
            return View(ads);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(Ad ad, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return View(ad);

            if (ImageFile != null)
            {
                using var ms = new MemoryStream();
                await ImageFile.CopyToAsync(ms);
                ad.Image = ms.ToArray();
            }

            ad.CreatedAt = DateTime.Now;
            ad.IsActive = true;
            _context.Add(ad);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إضافة الإعلان بنجاح";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null)
                return NotFound();

            return View(ad);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ad ad, IFormFile? ImageFile)
        {
            if (id != ad.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(ad);

            try
            {
                // التعامل مع تحديث الصورة إذا تم رفع صورة جديدة
                if (ImageFile != null)
                {
                    using var ms = new MemoryStream();
                    await ImageFile.CopyToAsync(ms);
                    ad.Image = ms.ToArray();
                }

                // تحديث الإعلان
                _context.Update(ad);
                await _context.SaveChangesAsync();

                TempData["Success"] = "تم تعديل الإعلان بنجاح";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdExists(ad.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null)
            {
                TempData["Error"] = "الإعلان غير موجود";
                return RedirectToAction(nameof(Index));
            }

            _context.Ads.Remove(ad);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم حذف الإعلان بنجاح";
            return RedirectToAction(nameof(Index));
        }

      
       
        public async Task<IActionResult> ToggleActive(int id)
        {
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null)
            {
                TempData["Error"] = "الإعلان غير موجود";
                return RedirectToAction(nameof(Index));
            }

            ad.IsActive = !ad.IsActive;
            _context.Update(ad);
            await _context.SaveChangesAsync();

            TempData["Success"] = ad.IsActive==true ? "تم تفعيل الإعلان" : "تم إلغاء تفعيل الإعلان";
            return RedirectToAction(nameof(Index));
        }


        private bool AdExists(int id) => _context.Ads.Any(e => e.Id == id);


        //-------------------------------------------------------------------------



 
        public async Task<IActionResult> IndexService()
        {
            var services = await _context.Services
                .Include(s => s.Type)
                .Include(s => s.Category)
                .OrderByDescending(s => s.Id)
                .ToListAsync();
            return View(services);
        }


        public IActionResult CreateService()
        {
            var vm = new ServiceViewModel
            {
                Types = _context.Codes.Where(x=>x.ParentId==7).ToList(),
                Categories = _context.Codes.Where(x => x.ParentId == 38).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(ServiceViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Types = _context.Codes.Where(x => x.ParentId == 7).ToList();
                vm.Categories = _context.Codes.Where(x => x.ParentId == 38).ToList();
                return View(vm);
            }

            var service = new Service
            {
                NameAr = vm.NameAr,
                NameEn = vm.NameEn,
                TypeId = vm.TypeId,
                CategoryId = vm.CategoryId
            };

            if (vm.ImageFile != null)
            {
                using var ms = new MemoryStream();
                await vm.ImageFile.CopyToAsync(ms);
                service.Image = ms.ToArray();
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تم إضافة الخدمة بنجاح";
            return RedirectToAction(nameof(IndexService));
        }

  
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            var vm = new ServiceViewModel
            {
                Id = service.Id,
                NameAr = service.NameAr,
                NameEn = service.NameEn,
                TypeId = service.TypeId,
                CategoryId = service.CategoryId,
                Image = service.Image,
                Types = _context.Codes.Where(x => x.ParentId == 7).ToList(),
                Categories = _context.Codes.Where(x => x.ParentId == 38).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(int id, ServiceViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                vm.Types = _context.Codes.Where(x => x.ParentId == 7).ToList();
                vm.Categories = _context.Codes.Where(x => x.ParentId == 38).ToList();
                return View(vm);
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            service.NameAr = vm.NameAr;
            service.NameEn = vm.NameEn;
            service.TypeId = vm.TypeId;
            service.CategoryId = vm.CategoryId;

            if (vm.ImageFile != null)
            {
                using var ms = new MemoryStream();
                await vm.ImageFile.CopyToAsync(ms);
                service.Image = ms.ToArray();
            }

            _context.Update(service);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تم تعديل الخدمة بنجاح";

            return RedirectToAction(nameof(IndexService));
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                TempData["Error"] = "الخدمة غير موجودة";
                return RedirectToAction(nameof(IndexService));
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تم حذف الخدمة بنجاح";
            return RedirectToAction(nameof(IndexService));
        }

     
        public async Task<IActionResult> ToggleActiveService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                TempData["Error"] = "الخدمة غير موجودة";
                return RedirectToAction(nameof(IndexService));
            }

            service.IsActive = !service.IsActive;
            _context.Update(service);
            await _context.SaveChangesAsync();

            TempData["Success"] = service.IsActive ? "تم تفعيل الخدمة" : "تم إلغاء تفعيل الخدمة";
            return RedirectToAction(nameof(IndexService));
        }




        //-------------------------------------------------------------------------


        public IActionResult IndexProduct()
        {
            List<Product> products;

            if (User.IsInRole("Admin"))
            {
        
                products = _context.Products
                                   .Include(p => p.Vendor)
                                   .Include(p => p.Category)
                                   .Include(p => p.ProductImages)
                                   .ToList();
            }
            else
            {
         
                var userName = User.Identity?.Name; // تحقق من null
                if (string.IsNullOrEmpty(userName))
                    return Unauthorized();

                var vendor = _context.Vendors
                                     .Include(v => v.Products)
                                         .ThenInclude(p => p.ProductImages)
                                     .Include(v => v.Products)
                                         .ThenInclude(p => p.Category)
                                     .FirstOrDefault(v => v.User.UserName == userName);

                if (vendor == null) return NotFound();

                products = vendor.Products?.ToList() ?? new List<Product>();
            }

            return View(products);
        }
      
        public IActionResult CreateProduc()
        {
            var vm = new ProductViewModel
            {
                Categories = _context.Categories.ToList(),
                VendorId = _context.Vendors.FirstOrDefault(v => v.User.UserName == User.Identity.Name)?.Id ?? 0,
                TotalProducts = _context.Products.Count(),
                ActiveProducts = _context.Products.Count(p => p.IsActive == true),
                InactiveProducts = _context.Products.Count(p => p.IsActive == false)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduc(ProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = _context.Categories.ToList();
                vm.TotalProducts = _context.Products.Count();
                vm.ActiveProducts = _context.Products.Count(p => p.IsActive==true);
                vm.InactiveProducts = _context.Products.Count(p => !p.IsActive==false);
                return View(vm);
            }

            var product = new Product
            {
                VendorId = vm.VendorId,
                CategoryId = vm.CategoryId,
                NameAr = vm.NameAr,
                NameEn = vm.NameEn,
                Description = vm.Description,
                PriceRangeMin = vm.PriceRangeMin,
                PriceRangeMax = vm.PriceRangeMax,
                IsActive = vm.IsActive
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (vm.ProductImages != null && vm.ProductImages.Any())
            {
                foreach (var file in vm.ProductImages)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);

                    var image = new ProductImage
                    {
                        ProductId = product.Id,
                        ImageData = ms.ToArray(),
                        IsMain = file.FileName == vm.MainImageFileName, // تحديد الصورة الرئيسية
                        CreatedAt = DateTime.Now
                    };
                    _context.ProductImages.Add(image);
                }
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "تم إضافة المنتج بنجاح";
            return RedirectToAction(nameof(IndexProduct));
        }


        public async Task<IActionResult> EditProduc(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.ProductImages)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            var vm = new ProductViewModel
            {
                Id = product.Id,
                VendorId = product.VendorId,
                CategoryId = product.CategoryId,
                NameAr = product.NameAr,
                NameEn = product.NameEn,
                Description = product.Description,
                PriceRangeMin = product.PriceRangeMin,
                PriceRangeMax = product.PriceRangeMax,
                IsActive = product.IsActive ?? true,
                Categories = _context.Categories.ToList(),
                ProductImages = new List<IFormFile>(), // ملفات جديدة
                TotalProducts = _context.Products.Count(),
                ActiveProducts = _context.Products.Count(p => p.IsActive == true),
                InactiveProducts = _context.Products.Count(p => p.IsActive == false)
            };

            ViewBag.ExistingImages = product.ProductImages; // للعرض والاختيار
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduc(int id, ProductViewModel vm, string? MainImageFileName)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Categories = _context.Categories.ToList();
                vm.TotalProducts = _context.Products.Count();
                vm.ActiveProducts = _context.Products.Count(p => p.IsActive == true);
                vm.InactiveProducts = _context.Products.Count(p => p.IsActive == false);
                return View(vm);
            }

            var product = await _context.Products
                                        .Include(p => p.ProductImages)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            product.NameAr = vm.NameAr;
            product.NameEn = vm.NameEn;
            product.CategoryId = vm.CategoryId;
            product.Description = vm.Description;
            product.PriceRangeMin = vm.PriceRangeMin;
            product.PriceRangeMax = vm.PriceRangeMax;
            product.IsActive = vm.IsActive;

            // رفع صور جديدة
            if (vm.ProductImages != null)
            {
                foreach (var file in vm.ProductImages)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    var image = new ProductImage
                    {
                        ProductId = product.Id,
                        ImageData = ms.ToArray(),
                        IsMain = false,
                        CreatedAt = DateTime.Now
                    };
                    _context.ProductImages.Add(image);
                }
            }

            // تحديد الصورة الرئيسية
            if (!string.IsNullOrEmpty(MainImageFileName))
            {
                foreach (var img in product.ProductImages)
                    img.IsMain = img.Id.ToString() == MainImageFileName; // افترضنا id الصورة أو يمكن التعديل حسب الاسم
            }

            _context.Update(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم تعديل المنتج بنجاح";
            return RedirectToAction(nameof(IndexProduct));
        }


        public async Task<IActionResult> DeleteProduc(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.ProductImages)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            _context.ProductImages.RemoveRange(product.ProductImages);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم حذف المنتج بنجاح";
            return RedirectToAction(nameof(IndexProduct));
        }


        public async Task<IActionResult> ToggleProductStatus(int id)
        {
            var product = await _context.Products
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            // عكس حالة التفعيل
            product.IsActive = !(product.IsActive ?? true);

            _context.Update(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = product.IsActive == true
                ? "تم تفعيل المنتج بنجاح"
                : "تم إلغاء تفعيل المنتج بنجاح";

            return RedirectToAction(nameof(IndexProduct));
        }





        //-------------------------------------------------------------------------


        public async Task<IActionResult> IndexCompanyType()
        {
            var codes = await _context.Codes
                                      .Where(c => c.ParentId == 7)
                                      .ToListAsync();
            return View(codes);
        }

        // إضافة
        public IActionResult CreateCompanyType()
        {
            var vm = new CodeViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompanyType(CodeViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var code = new Code
            {
                DescCodeAr = vm.DescCodeAr,
                DescCodeEn = vm.DescCodeEn,
                IsActive = vm.IsActive,
                ParentId = 7
            };

            // تحويل الصورة إلى byte[] إذا تم رفعها
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await vm.ImageFile.CopyToAsync(ms);
                code.Image = ms.ToArray();
            }

            _context.Codes.Add(code);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إضافة النوع بنجاح";
            return RedirectToAction(nameof(IndexCompanyType));
        }

        // تعديل
        public async Task<IActionResult> EditCompanyType(int id)
        {
            var code = await _context.Codes.FindAsync(id);
            if (code == null) return NotFound();

            var vm = new CodeViewModel
            {
                Id = code.Id,
                DescCodeAr = code.DescCodeAr,
                DescCodeEn = code.DescCodeEn,
                IsActive = code.IsActive ?? true,
                ParentId = 7, // تعديل ParentId دائمًا 7
                Image = code.Image            // لا تنسى إضافة حقل لعرض الصورة الحالية إذا حبيت تعرضها بالـ View
                                              // ImageFile = null // هذا فقط للرفع الجديد
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCompanyType(int id, CodeViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var code = await _context.Codes.FindAsync(id);
            if (code == null) return NotFound();

            code.DescCodeAr = vm.DescCodeAr;
            code.DescCodeEn = vm.DescCodeEn;
            code.IsActive = vm.IsActive;
            code.ParentId = 7;

            // معالجة رفع الصورة الجديدة
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await vm.ImageFile.CopyToAsync(ms);
                code.Image = ms.ToArray();
            }
            // إذا لم يرفع المستخدم صورة جديدة، تبقى الصورة القديمة محفوظة

            _context.Update(code);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم تعديل النوع بنجاح";
            return RedirectToAction(nameof(IndexCompanyType));
        }

        // تفعيل / إلغاء تفعيل
        public async Task<IActionResult> ToggleActiveCompanyType(int id)
        {
            var code = await _context.Codes.FindAsync(id);
            if (code == null) return NotFound();

            code.IsActive = !(code.IsActive ?? true);
            _context.Update(code);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تم تعديل حالة النوع بنجاح";
            return RedirectToAction(nameof(IndexCompanyType));
        }

        // حذف
        public async Task<IActionResult> DeleteCompanyType(int id)
        {
            var code = await _context.Codes.FindAsync(id);
            if (code == null) return NotFound();

            _context.Codes.Remove(code);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تم حذف النوع بنجاح";
            return RedirectToAction(nameof(IndexCompanyType));
        }

        //___________________________________________________________________________
        public async Task<IActionResult> AddServices()
        {
            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(v => v.User.UserName == User.Identity.Name);

            if (vendor == null)
                return Unauthorized();

            var services = await _context.Services
                .Include(s => s.Type)
                .Include(s => s.Category)
                .Where(s => s.IsActive &&
                       !_context.VendorServices
                        .Any(vs => vs.ServiceId == s.Id && vs.VendorId == vendor.Id))
                .OrderBy(s => s.NameAr)
                .ToListAsync();

            return View(services);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddServiceToVendor(int serviceId)
        {
            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(v => v.User.UserName == User.Identity.Name);

            if (vendor == null)
                return Unauthorized();

            var exists = await _context.VendorServices
                .AnyAsync(v => v.ServiceId == serviceId && v.VendorId == vendor.Id);

            if (exists)
            {
                TempData["Error"] = "الخدمة مضافة مسبقاً";
                return RedirectToAction(nameof(AddServices));
            }

            var vendorService = new VendorService
            {
                VendorId = vendor.Id,
                ServiceId = serviceId
            };

            _context.VendorServices.Add(vendorService);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تمت إضافة الخدمة بنجاح";

            return RedirectToAction(nameof(AddServices));
        }


        public async Task<IActionResult> MyServices()
        {
            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(v => v.User.UserName == User.Identity.Name);

            if (vendor == null)
                return Unauthorized();

            var vendorServices = await _context.VendorServices
                .Where(v => v.VendorId == vendor.Id)
                .Include(v => v.Service)
                .ThenInclude(s => s.Type)
                .Include(v => v.Service)
                .ThenInclude(s => s.Category)
                .ToListAsync();

            // تحويل List<VendorService> إلى List<Service>
            var services = vendorServices.Select(vs => vs.Service).ToList();

            return View(services); // الآن الـ View يستقبل IEnumerable<Service>
        }

       
        public async Task<IActionResult> RemoveServiceFromVendor(int serviceId)
        {
            // جلب البائع الحالي
            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(v => v.User.UserName == User.Identity.Name);

            if (vendor == null)
                return Unauthorized();

            // جلب السجل المرتبط بالخدمة والبائع
            var vendorService = await _context.VendorServices
                .FirstOrDefaultAsync(vs => vs.ServiceId == serviceId && vs.VendorId == vendor.Id);

            if (vendorService == null)
            {
                TempData["Error"] = "الخدمة غير موجودة أو تم حذفها مسبقاً";
                return RedirectToAction(nameof(MyServices));
            }

            // حذف الخدمة
            _context.VendorServices.Remove(vendorService);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم حذف الخدمة بنجاح";

            return RedirectToAction(nameof(MyServices));
        }

    }
}
