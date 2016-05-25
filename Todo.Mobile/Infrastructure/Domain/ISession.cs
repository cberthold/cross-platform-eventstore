using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public interface ISession
    {
        void Add<T>(T aggregate) where T : AggregateRoot;
        T Get<T>(Guid id, int? expectedVersion = null) where T : AggregateRoot;
        void Commit();
    }
}
