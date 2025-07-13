using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using clinic.Application.SD;
using clinic.Domain.models;
namespace clinic.Infrastructure.DatabaseInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly applicationContext _context;

        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, applicationContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            //Migrations
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    Console.WriteLine("Seeding started"); // visible in logs
                    _context.Database.Migrate();
                    // seed...
                    Console.WriteLine("Seeding finished");
                }


                // Adding Roles
                if (!_roleManager.RoleExistsAsync(Roles.AdminRole).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(Roles.AdminRole)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(Roles.NurseRole)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(Roles.DoctorRole)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(Roles.PatientRole)).GetAwaiter().GetResult();
                }

                // Add User Admin
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@clinic.com",
                    Email = "admin@clinic.com",
                    Name = "Adminstrator",
                    PhoneNumber = "1234567890",
                    Address = "Egypt",
                    City = "Cairo",
                    DOB = new DateOnly(2001, 9, 14),
                    About = "This is the admin user of the clinic application.",
                    Age = DateTime.Now.Year - 2001 ,

                }, "admin1").GetAwaiter().GetResult();

                ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@clinic.com");
                if (user != null)
                {
                    _userManager.AddToRoleAsync(user, Roles.AdminRole).GetAwaiter().GetResult();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return;

        }
    }
}
