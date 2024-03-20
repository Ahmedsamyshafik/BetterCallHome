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
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<View> views { get; set; }
        public DbSet<UserApartmentsReact> userApartmentsReacts { get; set; }
        public DbSet<UserApartmentsComment> userapartmentsComments { get; set; }
        public DbSet<NotificationTransaction> notificationTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.OwnedApartment)
            .WithOne(a => a.Owner)
            .HasForeignKey(a => a.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.CurrentLivingIn)
                .WithMany(a => a.StudentsApartment)
                .HasForeignKey(u => u.CurrentLivingInId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Apartment>().
                HasOne(o => o.Owner).
                WithMany(a => a.OwnedApartment).
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserApartmentsReact>()
                .HasOne(r => r.user)
                .WithMany(u => u.Reacts)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserApartmentsReact>()
                .HasOne(r => r.Apartment)
                .WithMany(a => a.Reacts)
                .HasForeignKey(r => r.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserApartmentsComment>()
                .HasOne(c => c.user)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserApartmentsComment>()
                .HasOne(c => c.Apartment)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }


    }
}
