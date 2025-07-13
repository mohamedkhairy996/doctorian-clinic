using clinic.Domain.models;
using clinic.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Infrastructure.Implementations
{
    public class PatientRepository : GenericRepository<Patient>,IPatientRepository
    {
        private readonly applicationContext _context;
        public PatientRepository(applicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
