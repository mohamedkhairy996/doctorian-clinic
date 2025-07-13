using clinic.Application.DTOs;
using clinic.Domain.models;
using clinic.Domain.Repositories;
using clinic.Infrastructure.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace clinic.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DoctorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var doctors = _unitOfWork.Doctor.GetAll(includeWords: "applicationUser");
            return Ok(doctors);
        }
        [HttpGet("{id}")]
        public IActionResult GetDoctorById(int id)
        {
            var doctor = _unitOfWork.Doctor.GetBy(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        [HttpPost]
        public IActionResult CreateDoctor(DoctorDTO doctor)
        {
            if (doctor == null)
            {
                return BadRequest("Doctor data is null");
            }
            var doc = new Doctor();
            doc.setContactInfo(doctor.ContactInfo);
            doc.setName(doctor.Name);
            doc.WorkingHours = (doctor.WorkingHours).ToString();
            doc.setSpecialization(doctor.Specialization);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return NotFound("User not found.");
            }
            doc.ApplicationUserId= currentUserId;
            _unitOfWork.Doctor.Add(doc);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetDoctorById), new { id = doc.Id }, doc);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(int id, DoctorDTO doctor)
        {
            if (doctor == null || doctor.DoctorId != id)
            {
                return BadRequest("Doctor data is invalid");
            }
            var existingDoctor = _unitOfWork.Doctor.GetBy(d => d.Id == id);
            if (existingDoctor == null)
            {
                return NotFound();
            }
            existingDoctor.setName(doctor.Name);
            existingDoctor.setSpecialization(doctor.Specialization);
            existingDoctor.WorkingHours = (doctor.WorkingHours).ToString();
            existingDoctor.setContactInfo(doctor.ContactInfo);
            _unitOfWork.Doctor.Update(existingDoctor);
            _unitOfWork.Complete();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            var doctor = _unitOfWork.Doctor.GetBy(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            _unitOfWork.Doctor.Delete(doctor);
            _unitOfWork.Complete();
            return NoContent();
        }
    }
}
