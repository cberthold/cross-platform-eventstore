using Infrastructure.Command;
using Infrastructure.Events;
using Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Bus
{
    public class InProcessBus : ICommandSender, IEventPublisher
    {
        readonly ICommandHandlerResolver commandResolver;
        readonly IEventHandlerResolver eventResolver;

        public InProcessBus(ICommandHandlerResolver commandResolver, IEventHandlerResolver eventResolver)
        {
            this.commandResolver = commandResolver;
            this.eventResolver = eventResolver;
        }



        public void Send<T>(T command) where T : ICommand
        {

            var handlers = commandResolver.ResolveCommandHandlers<T>(command);
            var handlersCount = (handlers == null) ? 0 : handlers.Count();
            if (handlersCount == 0)
            {
                throw new InvalidOperationException("No handler registered");
            }
            else if (handlersCount != 1)
            {
                throw new InvalidOperationException("Cannot send to more than one handler");
            }

            var handler = handlers.FirstOrDefault();
            handler.Handle(command);

        }

        public void Publish<T>(T @event) where T : IEvent
        {
            var handlers = eventResolver.ResolveEventHandlers(@event);

            if (handlers == null)
            {
                return;
            }

            foreach (var handler in handlers)
                handler(@event);

        }
    }

}
