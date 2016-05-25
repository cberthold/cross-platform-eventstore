using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.BoundedContext.Data;
using Todo.BoundedContext.Domain;

namespace Todo.BoundedContext.ReadModel
{
    public interface ITodoListReadModelFacade
    {
        TodoListDTO Get(Guid id);
    }
}
