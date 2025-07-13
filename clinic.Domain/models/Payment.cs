using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Domain.models
{
    public class Payment
    {
        public string?Token { get; set; }
        public long Amount { get; set; }
        public int ? AppointmentID { get; set; }
    }

}
