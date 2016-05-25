using Infrastructure.Domain;
using Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.BoundedContext.Events;

namespace Todo.BoundedContext.Domain
{
    public class TodoListAggregate : AggregateRoot
    {
        public override Guid Id { get; protected set; }
        public int LastOrder { get; protected set; }

        List<TodoItemEntity> items;
        public IList<TodoItemEntity> TodoItems
        {
            get { return items.ToList(); }
        }

        #region Aggregate creation

        private TodoListAggregate()
        {
            LastOrder = 0;
            items = new List<TodoItemEntity>();
        }
        protected TodoListAggregate(Guid id) : this()
        {
            Id = id;
            ApplyChange(new TodoListStarted());
        }

        public static TodoListAggregate Create(Guid id)
        {
            return new TodoListAggregate(id);
        }

        #endregion

        #region Domain Logic

        private TodoItemEntity FindItem(Guid itemId)
        {
            return items.FirstOrDefault(a => a.ItemId == itemId);
        }

        private void ValidateTodoItemExists(Guid itemId)
        {
            var item = FindItem(itemId);
            if (item == null) throw new DomainException($"Todo item with id {itemId} does not exist");
        }

        public void AddTodoItem(Guid newItemId, string title, bool completed)
        {
            int order = LastOrder + 1;
            ApplyChange(new TodoItemAdded(newItemId, title, completed, order));
        }

        public void UpdateTodoItem(Guid itemId, string title, bool completed)
        {
            ValidateTodoItemExists(itemId);
            ApplyChange(new TodoItemUpdated(itemId, title, completed));
        }

        public void RemoveTodoItem(Guid itemId)
        {
            ValidateTodoItemExists(itemId);
            ApplyChange(new TodoItemRemoved(itemId));
        }

        public void RemoveCompletedTodoItems()
        {
            if(items.Any(i=>i.Completed))
            {
                ApplyChange(new CompletedTodoItemsRemoved());
            }
        }

        #endregion

        #region Apply Domain Events

        public void Apply(TodoItemAdded @event)
        {
            var item = new TodoItemEntity(@event.ItemId, @event.Title, @event.Order, @event.Completed);
            items.Add(item);
            LastOrder = @event.Order;
        }

        public void Apply(TodoItemUpdated @event)
        {
            var item = FindItem(@event.ItemId);
            item.Update(@event.Title, @event.Completed);
        }

        public void Apply(TodoItemRemoved @event)
        {
            var item = FindItem(@event.ItemId);
            items.Remove(item);
        }

        public void Apply(CompletedTodoItemsRemoved @event)
        {
            items = (from i in items
                     where !i.Completed
                     select i).ToList();
        }

        #endregion



    }
}
