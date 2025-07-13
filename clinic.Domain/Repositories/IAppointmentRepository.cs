using clinic.Domain.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Domain.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime startTime);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateTime date);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId, DateTime date);
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);
        Task AddAsync(Appointment appointment);
    }
}
