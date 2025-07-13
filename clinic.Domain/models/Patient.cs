using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Domain.models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ContactInfo { get; set; }
        
        public string ? Email { get; set; }
        public string ? Address { get; set; }
        public DateOnly ? DOB { get; set; }
        public short? Age { get; set; }
        public string ApplicationUserId { get;  set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser User { get; set; }

    }
}
