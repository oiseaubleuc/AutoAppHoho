using System.Collections.Generic;
using AutoAppHoho.Models;




namespace AutoAppHoho.Models
{
    public class FuelType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Car> Cars { get; set; }
    }
}
