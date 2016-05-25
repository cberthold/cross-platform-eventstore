using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class AggregateAlreadyExistsException<T> : Exception
    {
        const string MESSAGE_FORMAT = "Domain object of type '{0}' with id '{1}' already exists";

        public AggregateAlreadyExistsException(Guid id)
            : base(string.Format(MESSAGE_FORMAT, typeof(T).Name, id))
        {

        }
    }
}
