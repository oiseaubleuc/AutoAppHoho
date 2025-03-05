using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AutoAppHoho.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Voornaam { get; set; }

        [Required]
        public string Achternaam { get; set; }

        public override string PhoneNumber { get; set; }



    }
}
