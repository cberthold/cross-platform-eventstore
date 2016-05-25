using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Command;
using Infrastructure.Events;
using Autofac.Core;
using Autofac;
using System.Reflection;

namespace Infrastructure.Bus
{
    public class AutofacHandlerResolver :
        IEventHandlerResolver,
        ICommandHandlerResolver
    {
        readonly ILifetimeScope lifetimeScope;

        public AutofacHandlerResolver(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public IEnumerable<ICommandHandler<TCommand>> ResolveCommandHandlers<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            return lifetimeScope.Resolve<IEnumerable<ICommandHandler<TCommand>>>();
        }

        public IEnumerable<Action<IEvent>> ResolveEventHandlers<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            var eventHandlers = lifetimeScope.Resolve<IEnumerable<IEventHandler<TEvent>>>();

            List<Action<IEvent>> actions = new List<Action<IEvent>>();
            foreach (var item in eventHandlers)
            {
                var handler = item;
                Action<IEvent> action = (x) => handler.Handle(((TEvent)x));
                actions.Add(action);
            }

            return actions;
        }
        
    }
}
