using Microsoft.AspNetCore.Mvc;
using AutoAppHoho.Data;
using AutoAppHoho.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AutoAppHoho.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public BookingController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ✅ **GET: Booking/Create**
        public IActionResult Create(int carId)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
            if (car == null)
            {
                return NotFound();
            }

            var booking = new Booking
            {
                CarId = carId,
                Car = car
            };

            return View(booking);
        }

        // ✅ **POST: Booking/Create**
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return View(booking);
            }

            DateTime appointmentDate = booking.BookingDate;
            int day = (int)appointmentDate.DayOfWeek; // 0 = zondag, 6 = zaterdag
            int hour = appointmentDate.Hour;
            int minutes = appointmentDate.Minute;

            // **🚫 Controle op werkdagen (alleen maandag - vrijdag)**
            if (day == 0 || day == 6)
            {
                ModelState.AddModelError("BookingDate", "U kunt alleen een afspraak maken van maandag tot vrijdag.");
                return View(booking);
            }

            // **🚫 Controle op toegestane uren (09:00 - 13:00 en 14:00 - 18:30)**
            if (!((hour >= 9 && hour < 13) || (hour >= 14 && (hour < 18 || (hour == 18 && minutes <= 30)))))
            {
                ModelState.AddModelError("BookingDate", "Kies een tijd tussen 09:00-13:00 of 14:00-18:30.");
                return View(booking);
            }

            // ✅ **Boeking opslaan**
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // ✅ **Bevestigingsmail verzenden**
            SendBookingConfirmationEmail(booking);

            // ✅ **Succesmelding tonen en doorsturen**
            TempData["SuccessMessage"] = "Uw reservatie is bevestigd! Controleer uw e-mail.";
            return RedirectToAction("Details", "Car", new { id = booking.CarId });
        }

        // ✅ **E-mailbevestiging versturen**
        private void SendBookingConfirmationEmail(Booking booking)
        {
            try
            {
                var smtpServer = _configuration["Email:SmtpServer"];
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
                var smtpUser = _configuration["Email:SmtpUser"];
                var smtpPass = _configuration["Email:SmtpPass"];

                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                var car = _context.Cars.FirstOrDefault(c => c.Id == booking.CarId);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("oiseaubleu899@gmail.com"),
                    Subject = "Bevestiging van uw reservatie",
                    Body = $@"
                        <h2>Reservatie Bevestigd!</h2>
                        <p>Beste {booking.FirstName} {booking.LastName},</p>
                        <p>Uw afspraak is bevestigd voor:</p>
                        <ul>
                            <li><strong>Datum:</strong> {booking.BookingDate:dddd, dd MMMM yyyy}</li>
                            <li><strong>Tijd:</strong> {booking.BookingDate:HH:mm}</li>
                            <li><strong>Auto:</strong> {car?.Name}</li>
                        </ul>
                        <p>Bedankt voor uw reservatie!</p>
                        <p>Met vriendelijke groeten,<br/>AutoAppHoho Team</p>
                    ",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(booking.Email);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fout bij het verzenden van de e-mail: {ex.Message}");
            }
        }
    }
}
