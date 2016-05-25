using Infrastructure.Events;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.BoundedContext.Data;
using Todo.BoundedContext.Events;

namespace Todo.BoundedContext.Projections
{
    public class TodoListDTOHandlers :
        IEventHandler<TodoListStarted>,
        IEventHandler<TodoItemAdded>,
        IEventHandler<TodoItemUpdated>,
        IEventHandler<TodoItemRemoved>,
        IEventHandler<CompletedTodoItemsRemoved>
    {

        IReadRepository<TodoListDTO> repository;

        public TodoListDTOHandlers(IReadRepository<TodoListDTO> repository)
        {
            this.repository = repository;
        }

        public void Handle(TodoItemRemoved message)
        {
            var todoList = repository.GetById(message.Id);
            todoList.Todos = (from i in todoList.Todos
                              where i.ItemId != message.ItemId
                              select i).ToList();
            repository.Update(todoList);
        }

        public void Handle(CompletedTodoItemsRemoved message)
        {
            var todoList = repository.GetById(message.Id);
            todoList.Todos = (from i in todoList.Todos
                              where !i.Completed
                              select i).ToList();
            repository.Update(todoList);
        }

        public void Handle(TodoItemUpdated message)
        {
            var todoList = repository.GetById(message.Id);
            var item = todoList.Todos.FirstOrDefault(i => i.ItemId == message.ItemId);
            item.Title = message.Title;
            item.Completed = message.Completed;
            repository.Update(todoList);
        }

        public void Handle(TodoItemAdded message)
        {
            var todoList = repository.GetById(message.Id);
            todoList.Todos.Add(new TodoItemDTO
            {
                ItemId = message.ItemId,
                Completed = message.Completed,
                Order = message.Order,
                Title = message.Title,
            });
            repository.Update(todoList);
        }

        public void Handle(TodoListStarted message)
        {
            var todoList = new TodoListDTO();
            todoList.Id = message.Id;
            repository.Insert(todoList);
        }
    }
}
