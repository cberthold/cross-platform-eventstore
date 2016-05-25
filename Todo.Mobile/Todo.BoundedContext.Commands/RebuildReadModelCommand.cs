using Infrastructure.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.BoundedContext.Commands
{
    public class RebuildReadModelCommand : ICommand
    {
        public int ExpectedVersion { get; set; }
    }
}
