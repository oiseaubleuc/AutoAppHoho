using System.Collections.Generic;
using AutoAppHoho.Models;



namespace AutoAppHoho.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string CarName { get; set; }
        public string CustomerName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
    }
}
