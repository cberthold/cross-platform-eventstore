using Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public interface IEventHandler<in T> : IHandler<T> where T : IEvent
    {
    }
}
