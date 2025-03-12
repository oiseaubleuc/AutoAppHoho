using AutoAppHoho.Data;
using AutoAppHoho.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using AutoAppHoho.Resources;

namespace AutoAppHoho.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public HomeController(ApplicationDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpPost]
        [HttpPost]
        [HttpGet]
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true, // Zorgt ervoor dat cookies niet worden geblokkeerd door de GDPR-instellingen
                        Path = "/"
                    }
                );
            }

            return LocalRedirect(returnUrl ?? "/");
        }



        public async Task<IActionResult> Index(string searchString, int? fuelTypeId, int? categoryId)
        {
            ViewData["Title"] = _localizer["Home"];
            ViewData["SearchPlaceholder"] = _localizer["SearchPlaceholder"];
            ViewData["FuelTypeLabel"] = _localizer["FuelType"];
            ViewData["CategoryLabel"] = _localizer["Category"];

            var cars = _context.Cars
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(c => c.Name.Contains(searchString));
            }

            if (fuelTypeId.HasValue)
            {
                cars = cars.Where(c => c.FuelTypeId == fuelTypeId);
            }

            if (categoryId.HasValue)
            {
                cars = cars.Where(c => c.CategoryId == categoryId);
            }

            ViewBag.FuelTypes = new SelectList(await _context.FuelTypes.ToListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

            return View(await cars.ToListAsync());
        }
    }
}
