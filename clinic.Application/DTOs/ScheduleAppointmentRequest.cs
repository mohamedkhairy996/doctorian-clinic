using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Application.DTOs
{
    public class ScheduleAppointmentRequest
    {
        public DateOnly Day { get; set; }
        public int DurationMinutes { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
