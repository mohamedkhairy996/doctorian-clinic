using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Application.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get;  set; }
        public DateTime StartTime { get;  set; }
        public DateOnly Day { get;  set; }
        public int DurationMinutes { get;  set; }
        public string Status { get;  set; } // "Pending", "Confirmed", "Cancelled"
        public string Diagnosis { get;  set; } // "Pending", "Confirmed", "Cancelled"
        public int PatientId { get;  set; }
        public int DoctorId { get;  set; }

    }
}
