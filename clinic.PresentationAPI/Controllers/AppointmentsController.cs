using clinic.Application.DTOs;
using clinic.Application.services;
using clinic.Domain.models;
using clinic.Domain.Repositories;
using clinic.Infrastructure.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Stripe;

namespace clinic.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentsController(IUnitOfWork unitOfWork)
        {
            StripeConfiguration.ApiKey = "sk_test_51RhuyvHGAZFtsgHbF7alWjDztAtQlamNbXBbyofc7uOn4aMR8if22ADVhl3VGHCFY48hS4cDoLwc86srEy44O4fz003IRDg0GB";

            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            try
            {
                // Retrieve all appointments
                var appointments = _unitOfWork.Appointment.GetAll(includeWords: "Patient,Doctor").OrderBy(a => a.Day);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            try
            {
                // Retrieve all appointments
                var appointment = _unitOfWork.Appointment.GetBy(app=>app.Id==id, inludeWords: "Patient,Doctor");
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("docId")]
        public async Task<IActionResult> GetAppointmentsByDoctor(int doctorId)
        {
            try
            {
                // Retrieve all appointments
                var appointments = _unitOfWork.Appointment.GetAll(a=>a.DoctorId==doctorId).OrderBy(a => a.Day);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("patId")]
        public async Task<IActionResult> GetAppointmentsByPatient(int patientId)
        {
            try
            {
                // Retrieve all appointments
                var appointments = _unitOfWork.Appointment.GetAll(a => a.PatientId == patientId, includeWords: "Patient,Doctor").OrderBy(a => a.Day);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("date")]
        public async Task<IActionResult> GetAppointmentsByDate(DateOnly date)
        {
            try
            {
                // Retrieve all appointments
                var appointments = _unitOfWork.Appointment.GetAll(a => a.Day == date).OrderBy(a => a.Day);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> ScheduleAppointment(ScheduleAppointmentRequest request)
        {
            try
            {
                var appointment = new Appointment();
                appointment.PatientId = request.PatientId;
                appointment.DoctorId = request.DoctorId;
                request.DurationMinutes = request.DurationMinutes == 0 ? 20 : request.DurationMinutes;
                appointment.DurationMinutes = request.DurationMinutes;
                appointment.Status = "Assigned";
                appointment.SetDay(request.Day);
                appointment.Diagnosis = "";
                var patient = _unitOfWork.Patient.GetBy(p => p.Id == request.PatientId);
                if (patient == null)
                {
                    return NotFound("Patient not found.");
                }
                var doctor = _unitOfWork.Doctor.GetBy(d => d.Id == request.DoctorId);
                if (doctor == null)
                {
                    return NotFound("Doctor not found.");
                }
                //// Check doctor availability
                //bool notAvailable = await _unitOfWork.Appointment.IsDoctorAvailableAsync(
                //    request.DoctorId, request.StartTime);
                //if (notAvailable)
                //{
                //    return Conflict("Doctor is not available at the requested time.");
                //}
                // Save the appointment
                await _unitOfWork.Appointment.AddAsync(appointment);
                 _unitOfWork.Complete();
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppointment(AppointmentDTO appointment)
        {
            try
            {
            //    if (id != appointment.AppointmentId)
            //    {
            //        return BadRequest("Appointment ID mismatch.");
            //    }
                var id = appointment.Id;
                var existingAppointment = _unitOfWork.Appointment.GetBy(a => a.Id == id);
                if (existingAppointment == null)
                {
                    return NotFound("Appointment not found.");
                }
                //// Update properties
                //existingAppointment.StartTime=DateTime.Now;
                existingAppointment.Status =  appointment.Status;
                existingAppointment.Diagnosis = appointment.Diagnosis;
                existingAppointment.SetDay(appointment.Day);
                if (appointment.Status == "Canceled" && existingAppointment.PaymentId != null)
                {
                    var refundService = new RefundService();
                    var refundOptions = new RefundCreateOptions
                    {
                        Charge = existingAppointment.PaymentId,
                        Reason = RefundReasons.RequestedByCustomer
                    };
                    Refund refund = refundService.Create(refundOptions);
                    existingAppointment.Status = "Refunded"; // or any appropriate status
                    existingAppointment.PaymentId = refund.Id; // Or store the original charge ID
                    _unitOfWork.Appointment.Update(existingAppointment);
                    _unitOfWork.Complete();
                    return Ok(new { success = true, refundId = refund.Id });

                }
                _unitOfWork.Appointment.Update(existingAppointment);
                _unitOfWork.Complete();
                 return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var appointment = _unitOfWork.Appointment.GetBy(a => a.Id == id);
                if (appointment == null)
                {
                    return NotFound("Appointment not found.");
                }
                _unitOfWork.Appointment.Delete(appointment);
                _unitOfWork.Complete();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}

