using Infrastructure.Command;
using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Bus
{
    public interface ICommandHandlerResolver
    {
        IEnumerable<ICommandHandler<TCommand>> ResolveCommandHandlers<TCommand>(TCommand command) where TCommand : ICommand;
        
    }
}
