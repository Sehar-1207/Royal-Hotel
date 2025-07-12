using Microsoft.EntityFrameworkCore;
using RoyalHillHotel.Models;

namespace RoyalHillHotel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Database 
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Staff> Staffs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Set default values or constraints
            modelBuilder.Entity<Room>()
            .Property(r => r.Status)
            .HasDefaultValue("Available");

            modelBuilder.Entity<Room>()
                .Property(r => r.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Staff>()
                .Property(s => s.Salary)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.RoomCharges)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.ExtraCharges)
                .HasColumnType("decimal(18,2)");

        }
    }
}