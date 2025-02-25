using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoAppHoho.Models;



namespace AutoAppHoho.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int FuelTypeId { get; set; }
        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; }
        public int Views { get; set; }
        // Navigation properties
        public FuelType? FuelType { get; set; }
        public Category? Category { get; set; }


        public string ImagePath { get; set; }



        //voor de map
        public string Location { get; set; }

    }
}
