﻿using AutoAppHoho.Data;
using AutoAppHoho.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace AutoAppHoho.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CarController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars.Include(c => c.FuelType).Include(c => c.Category).ToList();
            return View(cars);
        }

    
      
        public IActionResult Create()
        {
            ViewBag.FuelTypes = _context.FuelTypes.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

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
                        // File Upload Logic
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

            // Reload dropdown values in case of an error
            ViewBag.FuelTypes = _context.FuelTypes.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(car);
        }


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

    }
}
