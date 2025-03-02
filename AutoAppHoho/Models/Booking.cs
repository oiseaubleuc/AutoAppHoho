using System;
using System.ComponentModel.DataAnnotations;

namespace AutoAppHoho.Models
    {
        public class Booking
        {
            public int Id { get; set; }

            [Required]
            public int CarId { get; set; } // Link to Car

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public DateTime BookingDate { get; set; } // New Name for AppointmentDate

            public Car Car { get; set; } // Relationship with Car
        }
    }


