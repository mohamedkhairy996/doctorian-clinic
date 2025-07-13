using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Infrastructure.DatabaseInitializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}
