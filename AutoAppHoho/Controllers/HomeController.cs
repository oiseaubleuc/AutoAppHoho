using AutoAppHoho.Data;
using AutoAppHoho.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace AutoAppHoho.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
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
        public async Task<IActionResult> Index(string searchString, int? fuelTypeId, int? categoryId)
        {
            // Haal de auto's op
            var cars = _context.Cars
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .Where(c => !c.IsDeleted);

            // Filter op zoekterm
            if (!string.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(c => c.Name.Contains(searchString));
            }

            // Filter op brandstoftype
            if (fuelTypeId.HasValue)
            {
                cars = cars.Where(c => c.FuelTypeId == fuelTypeId);
            }

            // Filter op categorie
            if (categoryId.HasValue)
            {
                cars = cars.Where(c => c.CategoryId == categoryId);
            }

            // Stel de dropdown-opties in voor de filters
            ViewBag.FuelTypes = new SelectList(await _context.FuelTypes.ToListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

            return View(await cars.ToListAsync());
        }

       
    }
}

