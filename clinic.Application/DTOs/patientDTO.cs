using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Application.DTOs
{
    public class patientDTO
    {
        public string Name { set; get; }
        public DateOnly Dob { set; get; }
        public string ContactInfo { set; get; }
        public int PatientId { set; get; }
        public string Phone { set; get; }
    }
}
