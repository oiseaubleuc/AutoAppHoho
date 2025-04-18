using Microsoft.AspNetCore.Mvc;

namespace AutoAppHoho.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
