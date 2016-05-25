using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Exception
{
    public class ConcurrencyException : System.Exception
    {
        public ConcurrencyException(Guid id)
            : base($"A different version than expected was found in aggregate {id}")
        {
        }
    }
}
