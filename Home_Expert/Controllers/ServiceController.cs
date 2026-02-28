using Microsoft.AspNetCore.Mvc;

namespace Home_Expert.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
