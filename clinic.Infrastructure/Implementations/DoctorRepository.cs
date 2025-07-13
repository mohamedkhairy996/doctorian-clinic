using clinic.Domain.models;
using clinic.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Infrastructure.Implementations
{
    public class DoctorRepository : GenericRepository<Doctor>,IDoctorRepository
    {
        private readonly applicationContext _context;
        public DoctorRepository(applicationContext context) : base(context)
        {
            _context = context;
        }
       
    }
}
