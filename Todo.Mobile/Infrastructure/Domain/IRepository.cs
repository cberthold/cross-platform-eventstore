using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public interface IRepository
    {
        void Save<T>(T aggregate, int? expectedVersion = null) where T : AggregateRoot;
        T Get<T>(Guid aggregateId) where T : AggregateRoot;
    }
}
