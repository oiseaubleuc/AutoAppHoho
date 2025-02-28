using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoAppHoho.Models;

namespace AutoAppHoho.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; } = default!;
        public DbSet<FuelType> FuelTypes { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Nieuws> Nieuws { get; set; }
        public DbSet<Advertentie> Advertenties { get; set; }




        public DbSet<Admin> Admins { get; set; } = default!;
        public DbSet<Appointment> Appointments { get; set; } = default!;
        public DbSet<Invoice> Invoices { get; set; } = default!;






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding voor Category
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "SUV" },
                new Category { Id = 2, Name = "Sedan" },
                new Category { Id = 3, Name = "Hatchback" },
                new Category { Id = 4, Name = "Break" },
                new Category { Id = 5, Name = "Coupé" },
                new Category { Id = 6, Name = "Cabriolet" },
                new Category { Id = 7, Name = "Pick-up" }
            );

            // Seeding voor FuelType
            modelBuilder.Entity<FuelType>().HasData(
                new FuelType { Id = 1, Name = "Benzine" },
                new FuelType { Id = 2, Name = "Diesel" },
                new FuelType { Id = 3, Name = "Elektrisch" },
                new FuelType { Id = 4, Name = "Hybride" }
            );

          

        }
    }
}
