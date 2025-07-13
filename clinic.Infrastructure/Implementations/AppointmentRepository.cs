using clinic.Domain.models;
using clinic.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace clinic.Infrastructure.Implementations
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly applicationContext _context;
        public AppointmentRepository(applicationContext context) : base(context)
        {
            _context = context;
        }
        public Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            return _context.Appointments.FindAsync(appointmentId).AsTask();
        }

        public Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateTime date)
        {
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.StartTime.Date == date.Date)
                .ToListAsync()
                .ContinueWith(task => (IEnumerable<Appointment>)task.Result);
        }

        public Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId, DateTime date)
        {
            return _context.Appointments
                .Where(a => a.PatientId == patientId && a.StartTime.Date == date.Date)
                .ToListAsync()
                .ContinueWith(task => (IEnumerable<Appointment>)task.Result);
        }

        public Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime startTime)
        {
            return _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId &&
                               a.StartTime <= startTime &&
                               a.StartTime.AddMinutes(a.DurationMinutes) > startTime);
        }
        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
