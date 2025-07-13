using clinic.Application.DTOs;
using clinic.Domain.models;
using clinic.Domain.Repositories;
using clinic.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clinic.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PatientsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var patients = _unitOfWork.Patient.GetAll();
            return Ok(patients);
        }
        [HttpGet("{id}")]
        public IActionResult GetPatientById(int id)
        {

            var patient = _unitOfWork.Patient.GetBy(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }
        [HttpPost]
        public IActionResult CreatePatient(patientDTO patientdto)
        {
            if (patientdto == null)
            {
                return BadRequest("Patient data is null");
            }
            var patient = new Patient();
            patient.Name = patientdto.Name;
            patient.DOB = patientdto.Dob;
            patient.ContactInfo = patientdto.ContactInfo;
            _unitOfWork.Patient.Add(patient);
            _unitOfWork.Complete();
            return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, patientDTO patient)
        {
            if (patient == null || patient.PatientId != id)
            {
                return BadRequest("Patient data is invalid");
            }
            var existingPatient = _unitOfWork.Patient.GetBy(p => p.Id == id);
            if (existingPatient == null)
            {
                return NotFound();
            }
            existingPatient.Name = patient.Name;
            existingPatient.ContactInfo = patient.ContactInfo;
            existingPatient.DOB = patient.Dob;
            existingPatient.ContactInfo = patient.ContactInfo;
            existingPatient.PhoneNumber = patient.Phone;
            _unitOfWork.Patient.Update(existingPatient);
            _unitOfWork.Complete();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var patient = _unitOfWork.Patient.GetBy(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            _unitOfWork.Patient.Delete(patient);
            _unitOfWork.Complete();
            return NoContent();
        }
    }
}
