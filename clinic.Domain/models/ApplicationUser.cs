using Microsoft.AspNetCore.Identity;

namespace clinic.Domain.models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public DateOnly? DOB { get; set; }
        public double? Age { get; set; }
        public string? About { get; set; }
        public string? ContactInfo { get; set; }
    }
}
