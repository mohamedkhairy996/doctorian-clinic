using clinic.Domain.models;
using clinic.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Application.services
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentServices(
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Appointment> ScheduleAppointmentAsync(
            int patientId,
            int docId,
            DateTime startTime,
            int durationMinutes)
        {
            // Validate patient and doctor
            var patient = _patientRepository.GetBy(p=>p.Id==patientId);
            var doctor =  _doctorRepository.GetBy(d=>d.Id==docId);

            if (patient == null || doctor == null)
                throw new KeyNotFoundException("Patient or Doctor not found.");

            // Check doctor availability
            bool isAvailable = await _appointmentRepository.IsDoctorAvailableAsync(docId, startTime);
            if (!isAvailable)
                throw new InvalidOperationException("Doctor is not available at the requested time.");

            // Create and save appointment
            var appointment = new Appointment();
            appointment.StartTime= startTime;
            appointment.DurationMinutes=durationMinutes;
            appointment.Status = "Assigned";
            appointment.PatientId = patientId;
            appointment.DoctorId=docId;


            await _appointmentRepository.AddAsync(appointment);
            return appointment;
        }
        
    }
}
