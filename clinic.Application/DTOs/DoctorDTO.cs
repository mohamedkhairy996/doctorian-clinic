using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Application.DTOs
{
    public class DoctorDTO
    {
        public int? DoctorId { get;  set; }
        public string ? Name { get;  set; }
        public string ? Specialization { get;  set; }
        public int  WorkingHours { get;  set; }

        public string? ContactInfo { get;  set; }
    }
}
