using Home_Expert.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Home_Expert.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LanguageController> _logger;
        public LanguageController(ApplicationDbContext context, ILogger<LanguageController> logger)
        {
            _logger = logger;
            _context = context;

        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
       
    }
}
