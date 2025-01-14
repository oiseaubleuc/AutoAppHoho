using System;
using System.ComponentModel.DataAnnotations;

namespace AutoAppHoho.Models
{
    public class Nieuws
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Titel { get; set; }

        [Required]
        public string Tekst { get; set; }

        [Required]
        public DateTime Publicatiedatum { get; set; }
        
        public string? ImagePath { get; set; }

     
    }
}
