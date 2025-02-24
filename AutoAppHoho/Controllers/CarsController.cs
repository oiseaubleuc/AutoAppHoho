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

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 INDEX - Toon alle auto's behalve verwijderde auto's
        public async Task<IActionResult> Index(string search)
        {
            var cars = _context.Cars
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                cars = cars.Where(c => c.Name.Contains(search) || c.FuelType.Name.Contains(search));
            }

            return View(await cars.ToListAsync());
        }

        // 🔹 DETAILS - Toon details van een auto
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var car = await _context.Cars
                .Include(c => c.FuelType)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null) return NotFound();

            // ✅ Aantal views bijhouden
            car.Views++;
            _context.Update(car);
            await _context.SaveChangesAsync();

            return View(car);
        }

        // 🔹 CREATE - Formulier voor nieuwe auto tonen
        public IActionResult Create()
        {
            PopulateDropdowns(); // Dropdowns vullen
            return View();
        }

        // 🔹 CREATE - Auto opslaan in database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,FuelTypeId,CategoryId,Description,Location,ImagePath")] Car car)
        {
            if (ModelState.IsValid)
            {
                car.IsDeleted = false; // Zorg ervoor dat de auto actief is bij het aanmaken
                car.Views = 0; // Start met 0 views

                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(car);
            return View(car);
        }

        // 🔹 EDIT - Formulier voor bewerken auto
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            PopulateDropdowns(car);
            return View(car);
        }

        // 🔹 EDIT - Auto bijwerken in database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(car);
            return View(car);
        }

        // 🔹 DELETE - Bevestigingspagina voor verwijderen
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            return View(car);
        }

        // 🔹 DELETE - Auto markeren als verwijderd
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                car.IsDeleted = true; // ✅ Soft delete
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 🔹 Dropdowns voor FuelType en Category instellen
        private void PopulateDropdowns(Car? car = null)
        {
            ViewBag.FuelTypes = new SelectList(_context.FuelTypes.OrderBy(f => f.Name), "Id", "Name", car?.FuelTypeId);
            ViewBag.Categories = new SelectList(_context.Categories.OrderBy(c => c.Name), "Id", "Name", car?.CategoryId);
        }
    }
}
