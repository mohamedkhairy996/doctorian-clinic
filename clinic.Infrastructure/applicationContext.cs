using clinic.Domain.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace clinic.Infrastructure
{
    public class applicationContext : IdentityDbContext<ApplicationUser>
    {
        public applicationContext(DbContextOptions<applicationContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships, indexes, etc. here
            base.OnModelCreating(modelBuilder);
        }
        // DbSets for your entities
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        // Add DbSet for ApplicationUser if needed
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        // Add other DbSets as needed
    }
}
