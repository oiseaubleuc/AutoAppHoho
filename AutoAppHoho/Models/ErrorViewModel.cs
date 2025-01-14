using System.Collections.Generic;
using AutoAppHoho.Models;




namespace AutoAppHoho.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
