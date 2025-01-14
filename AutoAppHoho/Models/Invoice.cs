using System.Collections.Generic;
using AutoAppHoho.Models;



namespace AutoAppHoho.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string CarName { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
