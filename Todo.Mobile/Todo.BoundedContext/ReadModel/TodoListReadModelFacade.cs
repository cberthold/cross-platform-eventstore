using Infrastructure.Domain;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.BoundedContext.Data;
using Todo.BoundedContext.Domain;

namespace Todo.BoundedContext.ReadModel
{
    public class TodoListReadModelFacade : ITodoListReadModelFacade
    {
        public readonly IReadRepository<TodoListDTO> repository;

        public TodoListReadModelFacade(IReadRepository<TodoListDTO> repository)
        {
            this.repository = repository;
        }

        public TodoListDTO Get(Guid id)
        {
            return repository.GetById(id);
        }
    }
}
