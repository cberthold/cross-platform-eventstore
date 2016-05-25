using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.BoundedContext.Events
{
    public class TodoItemUpdated : IEvent
    {
        public Guid Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public int Version { get; set; }

        public TodoItemUpdated(Guid itemId, string title, bool completed)
        {
            ItemId = itemId;
            Title = title;
            Completed = completed;
        }

        public Guid ItemId { get; private set; }
        public string Title { get; private set; }
        public bool Completed { get; private set; }
    }
}
