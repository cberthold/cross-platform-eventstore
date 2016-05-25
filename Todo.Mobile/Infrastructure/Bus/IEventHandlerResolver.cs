using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Bus
{
    public interface IEventHandlerResolver
    {
        IEnumerable<Action<IEvent>> ResolveEventHandlers<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
