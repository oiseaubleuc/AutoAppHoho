using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoAppHoho.Data;
using AutoAppHoho.Models;


namespace AutoAppHoho.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarsController> _logger;

        public CarsController(ApplicationDbContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string search, string sortOrder)
        {
            var cars = _context.Cars
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                cars = cars.Where(c => c.Name.Contains(search) || c.FuelType.Name.Contains(search));
            }

            ViewData["NameSort"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["PriceSort"] = sortOrder == "price" ? "price_desc" : "price";

            switch (sortOrder)
            {
                case "name":
                    cars = cars.OrderBy(c => c.Name);
                    break;
                case "name_desc":
                    cars = cars.OrderByDescending(c => c.Name);
                    break;
                case "price":
                    cars = cars.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    cars = cars.OrderByDescending(c => c.Price);
                    break;
            }

            return View(await cars.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var car = await _context.Cars
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null) return NotFound();

            return View(car);
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,FuelTypeId,CategoryId")] Car car)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(car);
                return View(car);
            }

            try
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Auto succesvol toegevoegd!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij opslaan van auto: {ex.Message}");
                ModelState.AddModelError("", "Fout bij opslaan, probeer opnieuw.");
            }

            PopulateDropdowns(car);
            return View(car);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            PopulateDropdowns(car);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,FuelTypeId,CategoryId")] Car car)
        {
            if (id != car.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateDropdowns(car);
                return View(car);
            }

            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Auto succesvol bijgewerkt!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij bewerken van auto: {ex.Message}");
                ModelState.AddModelError("", "Fout bij bewerken, probeer opnieuw.");
            }

            PopulateDropdowns(car);
            return View(car);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var car = await _context.Cars
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null) return NotFound();

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            try
            {
                car.IsDeleted = true;
                _context.Update(car);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Auto succesvol verwijderd!";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij verwijderen van auto: {ex.Message}");
                TempData["ErrorMessage"] = "Fout bij verwijderen, probeer opnieuw.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        private void PopulateDropdowns(Car? car = null)
        {
            ViewBag.FuelTypes = new SelectList(_context.FuelTypes.OrderBy(f => f.Name), "Id", "Name", car?.FuelTypeId);
            ViewBag.Categories = new SelectList(_context.Categories.OrderBy(c => c.Name), "Id", "Name", car?.CategoryId);
        }
    }
}
