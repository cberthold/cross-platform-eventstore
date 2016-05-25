using Infrastructure.Command;
using Infrastructure.Domain;
using Infrastructure.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.BoundedContext.Commands;
using Todo.BoundedContext.Domain;

namespace Todo.BoundedContext.Handlers
{
    public class TodoCommandHandlers :
        ICommandHandler<AddTodoItem>,
        ICommandHandler<RemoveTodoItem>,
        ICommandHandler<UpdateTodoItem>,
        ICommandHandler<RemoveCompletedTodoItems>
    {
        readonly ISession session;

        public TodoCommandHandlers(ISession session)
        {
            this.session = session;
        }

        private TodoListAggregate GetOrCreateTodoList(Guid tenantId)
        {
            try
            {
                var list = session.Get<TodoListAggregate>(tenantId);
                return list;
            }
            catch (AggregateNotFoundException ex)
            {
                // happens the first time a tenant creates a todo list item
                var list = TodoListAggregate.Create(tenantId);
                //add the aggregate to session
                session.Add(list);
                return list;
            }
            
        }

        public void Handle(AddTodoItem message)
        {
            var aggregate = GetOrCreateTodoList(message.TenantId);
            session.Add(aggregate);
            aggregate.AddTodoItem(message.ItemId, message.Title, message.Completed);
            session.Commit();
        }

        public void Handle(RemoveTodoItem message)
        {
            var aggregate = GetOrCreateTodoList(message.TenantId);
            session.Add(aggregate);
            aggregate.RemoveTodoItem(message.ItemId);
            session.Commit();
        }

        public void Handle(UpdateTodoItem message)
        {
            var aggregate = GetOrCreateTodoList(message.TenantId);
            session.Add(aggregate);
            aggregate.UpdateTodoItem(message.ItemId, message.Title, message.Completed);
            session.Commit();
        }

        public void Handle(RemoveCompletedTodoItems message)
        {
            var aggregate = GetOrCreateTodoList(message.TenantId);
            session.Add(aggregate);
            aggregate.RemoveCompletedTodoItems();
            session.Commit();
        }
    }
}
