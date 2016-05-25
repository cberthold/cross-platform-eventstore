using Infrastructure.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Factories
{
    public interface IAggregateFactory
    {
        T CreateAggregate<T>();
    }
}
