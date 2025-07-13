namespace clinic.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {

        IApplicationUserRepository ApplicationUser { get; }
        IAppointmentRepository Appointment { get; }
        IDoctorRepository Doctor { get; }
        IPatientRepository Patient { get; }

        int Complete();
    }
    
    
}
