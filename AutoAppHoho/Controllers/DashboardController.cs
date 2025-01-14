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
        try
        {
            var dashboardViewModel = new DashboardViewModel
            {
                TotalCars = await _context.Cars.CountAsync(),
                TotalAppointments = await _context.Appointments.CountAsync(),
                TotalAdmins = await _context.Admins.CountAsync(),
                TotalInvoices = await _context.Invoices.CountAsync(),
                RecentCars = await _context.Cars.OrderByDescending(c => c.Id).Take(5).ToListAsync(),
                RecentAppointments = await _context.Appointments.OrderByDescending(a => a.AppointmentDate).Take(5).ToListAsync(),
            };

            return View(dashboardViewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading dashboard: {ex.Message}");
            throw;
        }
    }
}
