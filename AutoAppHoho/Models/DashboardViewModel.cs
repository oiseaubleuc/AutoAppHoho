using System.Collections.Generic;
using AutoAppHoho.Models;

namespace AutoAppHoho.Models
{
    public class DashboardViewModel
    {
        public int TotalCars { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalInvoices { get; set; }
        public IEnumerable<Car> RecentCars { get; set; }
        public IEnumerable<Appointment> RecentAppointments { get; set; }

    }
}
