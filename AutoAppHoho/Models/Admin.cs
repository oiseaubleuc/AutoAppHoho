using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AutoAppHoho.Models;


namespace AutoAppHoho.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public string Role { get; set; } = "Admin"; 
    }
}
