using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace clinic.Domain.models
{
    // Appointment.cs
    public class Appointment
    {
        [Key] public int Id { get; set; }
        public DateTime StartTime { get;  set; }
        public DateOnly Day { get; private set; }
        public int DurationMinutes { get;  set; }
        public string Status { get;  set; } // "Pending", "Confirmed", "Cancelled"
        public string Diagnosis { get; set; } 
        public string? PaymentId { get; set; }

        // Foreign Keys
        public int PatientId { get;  set; }
        public int DoctorId { get;  set; }


        // Navigation Properties (EF Core)
        public virtual Patient Patient { get; private set; }
        public virtual Doctor Doctor { get; private set; }


        public void SetDay(DateOnly day)
        {
            if (day < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException("Date cannot be earlier than today.");
            }

            this.Day = day;
        }

        public void Confirm() => Status = "Confirmed";

        public void Cancel() => Status = "Cancelled";
    }
}
