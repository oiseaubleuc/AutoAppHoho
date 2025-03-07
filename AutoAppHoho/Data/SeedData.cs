using AutoAppHoho.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AutoAppHoho.Data
{
    public static class SeedData
    {
        public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@autoapphoho.com";

            // Controleer of de admin al bestaat
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(), 
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Achternaam = "Administrator", 
                    Voornaam = "Super", 
                    PhoneNumber = "0123456789" 
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                    }
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}