using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.BoundedContext.Events
{
    public class TodoItemAdded : IEvent
    {
        public Guid Id { get; set; }
       
        public DateTimeOffset TimeStamp { get; set; }

        public int Version { get; set; }

        public TodoItemAdded(Guid itemId, string title, bool completed, int order)
        {
            ItemId = itemId;
            Title = title;
            Completed = completed;
            Order = order;
        }

        public Guid ItemId { get; private set; }
        public string Title { get; private set; }
        public bool Completed { get; private set; }
        public int Order { get; private set; }
    }
}
