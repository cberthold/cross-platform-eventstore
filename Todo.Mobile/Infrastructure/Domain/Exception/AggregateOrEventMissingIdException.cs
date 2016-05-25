using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Exception
{
    public class AggregateOrEventMissingIdException : System.Exception
    {
        public AggregateOrEventMissingIdException(Type aggregateType, Type eventType)
            : base($"An event of type {eventType.FullName} was tried to save from {aggregateType.FullName} but no id where set on either")
        {

        }
    }
}
