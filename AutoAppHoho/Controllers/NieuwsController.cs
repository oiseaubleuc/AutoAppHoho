using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using AutoAppHoho.Data;
using AutoAppHoho.Models;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.ConstrainedExecution;

namespace AutoAppHoho.Controllers
{
    public class NieuwsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NieuwsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Nieuws
        public async Task<IActionResult> Index()
        {
            var news = await _context.Nieuws
                .OrderByDescending(n => n.Publicatiedatum)
                .Select(n => new Nieuws
                {
                    Id = n.Id,
                    Titel = n.Titel,
                    Tekst = n.Tekst,
                    Publicatiedatum = n.Publicatiedatum,
                    ImagePath = string.IsNullOrEmpty(n.ImagePath) ? "/images/default-news.jpg" : n.ImagePath
                }).ToListAsync();

            return View(news);
        }


        // GET: Nieuws/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var news = await _context.Nieuws.FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        // GET: Nieuws/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nieuws/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Nieuws news, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                string uniqueFileName = null;
                

                if (image != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    news.ImagePath = $"/uploads/{uniqueFileName}";
                }

                news.Publicatiedatum = DateTime.Now;
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(news);
        }

        // GET: Nieuws/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var news = await _context.Nieuws.FindAsync(id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        // POST: Nieuws/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Nieuws news, IFormFile image)
        {
            if (id != news.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        // Verwijder de oude afbeelding als deze bestaat
                        if (!string.IsNullOrEmpty(news.ImagePath))
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, news.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        
                        news.ImagePath = "/uploads/" + uniqueFileName;
                    }
                    
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(news);
        }

        // GET: Nieuws/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var news = await _context.Nieuws.FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        // POST: Nieuws/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.Nieuws.FindAsync(id);
            if (news != null)
            {
                
                if (!string.IsNullOrEmpty(news.ImagePath))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, news.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                
                _context.Nieuws.Remove(news);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.Nieuws.Any(e => e.Id == id);
        }
    }
}