using AutoAppHoho.Data;
using AutoAppHoho.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAppHoho.Controllers
{
    [Authorize] // 🔒 Alleen ingelogde gebruikers kunnen auto's beheren
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IViewLocalizer _localizer;

        public CarController(ApplicationDbContext context, IWebHostEnvironment environment, IViewLocalizer localizer)
        {
            _context = context;
            _environment = environment;
            _localizer = localizer;

        }

        // ✅ 🔹 Overzicht van alle auto's
        [AllowAnonymous] // 🚀 Iedereen mag de lijst bekijken
        public async Task<IActionResult> Index(int? fuelTypeId, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var cars = _context.Cars
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .AsQueryable();

            if (fuelTypeId.HasValue)
            {
                cars = cars.Where(c => c.FuelTypeId == fuelTypeId.Value);
            }

            if (categoryId.HasValue)
            {
                cars = cars.Where(c => c.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                cars = cars.Where(c => c.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                cars = cars.Where(c => c.Price <= maxPrice.Value);
            }

            ViewBag.FuelTypes = await _context.FuelTypes.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(await cars.ToListAsync());
        }

        // ✅ 🔹 Nieuwe auto aanmaken (GET)
        public IActionResult Create()
        {
            ViewBag.FuelTypes = _context.FuelTypes.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // ✅ 🔹 Nieuwe auto aanmaken (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (car.ImageFile != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(car.ImageFile.FileName);
                        var extension = Path.GetExtension(car.ImageFile.FileName);
                        var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await car.ImageFile.CopyToAsync(stream);
                        }

                        car.ImagePath = $"/uploads/{uniqueFileName}";
                    }

                    _context.Add(car);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Database error occurred while saving the car. Please try again.");
                Console.WriteLine($"DB Error: {ex.Message}");
            }
            catch (IOException ex)
            {
                ModelState.AddModelError("", "Error uploading image. Please try a different image file.");
                Console.WriteLine($"File Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                Console.WriteLine($"General Error: {ex.Message}");
            }

            ViewBag.FuelTypes = _context.FuelTypes.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(car);
        }

        // ✅ 🔹 Auto details bekijken
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var car = await _context.Cars
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        // ✅ 🔹 Auto bewerken (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            ViewBag.FuelTypes = _context.FuelTypes.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(car);
        }

        // ✅ 🔹 Auto bewerken (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(car);

            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cars.Any(e => e.Id == car.Id))
                    return NotFound();

                throw;
            }
        }

        // ✅ 🔹 Auto verwijderen (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        // ✅ 🔹 Auto verwijderen (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ✅ 🔹 Auto zoeken
        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var cars = await _context.Cars
                .Where(c => c.Name.Contains(searchTerm) || c.FuelType.Name.Contains(searchTerm))
                .Select(c => new { c.Id, c.Name, c.Price, FuelType = c.FuelType.Name })
                .ToListAsync();

            return Json(cars);
        }
    }
}
