using clinic.Application.SD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Application.DTOs
{
    public class ApplicationUserDTO
    {
        public string username { get; set; }
        public string password { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? City { get; set; }
        public DateOnly? DOB { get; set; }
        public string? About { get; set; }
        public string? ContactInfo { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? Specialization { get; set; }
        public string? Role { get; set; } = Roles.PatientRole;

    }
}
