using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Command;
using Infrastructure.Events;
using System.Reflection;

namespace Infrastructure.Bus
{
    public class MobileHandlerResolver :
        IEventHandlerResolver,
        ICommandHandlerResolver
    {
        readonly AutofacHandlerResolver resolver;
        readonly static Type resolverType = typeof(AutofacHandlerResolver);
        readonly static TypeInfo resolverTypeInfo = resolverType.GetTypeInfo();
        readonly static MethodInfo resolveEventHandlersMethod = resolverTypeInfo.GetDeclaredMethod("ResolveEventHandlers");
        public MobileHandlerResolver(AutofacHandlerResolver resolver)
        {
            this.resolver = resolver;

        }

        public IEnumerable<ICommandHandler<TCommand>> ResolveCommandHandlers<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            return resolver.ResolveCommandHandlers(command);
        }

        public IEnumerable<Action<IEvent>> ResolveEventHandlers<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            var type = @event.GetType();

            var generic = resolveEventHandlersMethod.MakeGenericMethod(type);
            var actions = (IEnumerable<Action<IEvent>>)generic.Invoke(resolver, new object[] { @event });
            
            return actions;
        }
    }
}
