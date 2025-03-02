using AutoAppHoho.Data;
using AutoAppHoho.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAppHoho.Controllers
{
    public class MapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MapController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index Action (to display map)

        public IActionResult Index()
        {
            var cars = _context.Cars.ToList();

            if (!cars.Any())
            {
                ViewBag.ErrorMessage = "Geen auto's gevonden.";
            }

            return View(cars);
        }



        // Create Action (to add a new car)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(car);
        }
    }
}
