using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Exception
{
    public class AggregateNotFoundException : System.Exception
    {
        public AggregateNotFoundException(Guid id)
            : base($"Aggregate {id} was not found")
        {
        }
    }
}
