using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinic.Domain.models
{
    // Doctor.cs
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get;  set; }
        public string Specialization { get;  set; }
        public string? WorkingHours { get; set; } 

        public string? ContactInfo { get;  set; }
        public string ApplicationUserId { get;  set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser applicationUser { get;  set; }

        public void setName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            Name = name;
        }
        public void setSpecialization(string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                throw new ArgumentException("Specialization cannot be empty", nameof(specialization));
            Specialization = specialization;
        }
        public void setContactInfo(string contactInfo)
        {
            if (string.IsNullOrWhiteSpace(contactInfo))
                throw new ArgumentException("Contact information cannot be empty", nameof(contactInfo));
            ContactInfo = contactInfo;
        }
        public void setDoctorId(int id)
        {
            if (id == 0)
                throw new ArgumentException("Doctor ID cannot be empty", nameof(id));
            Id = id;
        }
    }
}
