
using clinic.Domain.Repositories;

namespace clinic.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly applicationContext _context;
        public IApplicationUserRepository ApplicationUser { get; }
        public IAppointmentRepository Appointment { get; }
        public IDoctorRepository Doctor { get; }
        public IPatientRepository Patient { get; }

        public UnitOfWork(applicationContext context)
        {
            _context = context;
            Appointment = new AppointmentRepository(_context);
            Doctor = new DoctorRepository(_context);
            Patient = new PatientRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);

        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
