using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using AutoAppHoho.Models;

namespace AutoAppHoho.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider,
                                            UserManager<ApplicationUser> userManager,
                                            RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string adminEmail = "admin@autoapphoho.com";
            string adminPassword = "Admin123!";
            string auto =  "auto";
            string School = "School";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    Voornaam = auto,
                    Achternaam = School, 
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                var standaardUser = new ApplicationUser
                {
                    Voornaam = "houdi",
                    Achternaam = "leerling",
                    UserName = "user@autoapphoho.com",
                    Email = "user@autoapphoho.com",
                    EmailConfirmed = true
                };

                if (await userManager.FindByEmailAsync(standaardUser.Email) == null)
                {
                    var createUser = await userManager.CreateAsync(standaardUser, "User123!");
                    if (createUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(standaardUser, "User");
                    }
                }

            }
        }
    }
}
