using Domin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Apartment> Apartment { get; set; }
        public DbSet<ApartmentImages> apartmentImages { get; set; }
        public DbSet<RoyalDocument> royalDocuments { get; set; }
        public DbSet<ApartmentVideo> apartmentVideos { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ApartmentServices> ApartmentServices { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.OwnedApartment)
            .WithOne(a => a.Owner)
            .HasForeignKey(a => a.OwnerId);

            modelBuilder.Entity<ApplicationUser>()
           .HasOne(u => u.CurrentLivingIn)
           .WithMany(a => a.StudentsApartment)
           .HasForeignKey(u => u.CurrentLivingInId)
           .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }


    }
}
