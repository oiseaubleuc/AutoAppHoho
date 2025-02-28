using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoAppHoho.Data;
using AutoAppHoho.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class AdvertentiesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public AdvertentiesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
    }

    // GET: Advertentie/Index
    public async Task<IActionResult> Index()
    {
        return View(await _context.Advertenties.ToListAsync());
    }

    // GET: Advertentie/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var advertentie = await _context.Advertenties
            .FirstOrDefaultAsync(m => m.Id == id);
        if (advertentie == null)
        {
            return NotFound();
        }

        return View(advertentie);
    }

    // GET: Advertentie/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Advertentie/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Advertentie advertentie, IFormFile imageFile)
    {
        if (!ModelState.IsValid)
        {
            string uniqueFileName = null;


            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "wwwroot/uploads/news");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                advertentie.ImagePath = "/uploads/news" + uniqueFileName;
            }

            _context.Add(advertentie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(advertentie);
    }

    // GET: Advertentie/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var advertentie = await _context.Advertenties.FindAsync(id);
        if (advertentie == null)
        {
            return NotFound();
        }
        return View(advertentie);
    }

    // POST: Advertentie/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Advertentie advertentie, IFormFile imageFile)
    {
        if (id != advertentie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    advertentie.ImagePath = "/images/" + uniqueFileName;
                }

                _context.Update(advertentie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertentieExists(advertentie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(advertentie);
    }

    // GET: Advertentie/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var advertentie = await _context.Advertenties
            .FirstOrDefaultAsync(m => m.Id == id);
        if (advertentie == null)
        {
            return NotFound();
        }

        return View(advertentie);
    }

    // POST: Advertentie/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var advertentie = await _context.Advertenties.FindAsync(id);
        _context.Advertenties.Remove(advertentie);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AdvertentieExists(int id)
    {
        return _context.Advertenties.Any(e => e.Id == id);
    }
}