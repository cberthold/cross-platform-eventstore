using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public interface IEventStore
    {
        void Save(IEvent @event);
        IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion);
    }
}
