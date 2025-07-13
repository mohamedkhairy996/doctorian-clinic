using clinic.Domain.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Application.services
{
    public interface IAppointmentServices
    {
        Task<Appointment> ScheduleAppointmentAsync(int patientId,
            int doctorId,
            DateTime startTime,
            int durationMinutes);
        // Other method signatures...
    }
}
