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
        public List<Car> RecentCars { get; set; }
        public List<Appointment> RecentAppointments { get; set; }

        public List<CarLocationViewModel> CarLocations { get; set; }
        public IEnumerable<CarOffer> CarOffers { get; set; } 

    }

    public class CarLocationViewModel
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
