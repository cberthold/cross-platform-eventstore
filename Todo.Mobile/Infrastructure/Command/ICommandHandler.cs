using Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Command
{
    public interface ICommandHandler<in T> : IHandler<T> where T : ICommand
    {
    }
}
