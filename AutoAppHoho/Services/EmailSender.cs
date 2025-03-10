using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace AutoAppHoho.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"Simulated sending email to {email} with subject {subject}");
            return Task.CompletedTask;
        }
    }
}
