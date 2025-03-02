using AutoAppHoho.Data;
using AutoAppHoho.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var totalCars = await _context.Cars.CountAsync();
        var totalInvoices = await _context.Invoices.CountAsync();

        var recentCars = await _context.Cars
            .Include(c => c.FuelType)
            .Include(c => c.Category)
            .OrderByDescending(c => c.Id)
            .Take(5)
            .ToListAsync();

        ViewBag.TotalCars = totalCars;
        ViewBag.TotalInvoices = totalInvoices;
        ViewBag.RecentCars = recentCars ?? new List<Car>(); // ✅ Ensure it's never null

        return View();
    }
}
