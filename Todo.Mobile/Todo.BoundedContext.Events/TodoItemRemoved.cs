using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.BoundedContext.Events
{
    public class TodoItemRemoved : IEvent
    {
        public Guid Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public int Version { get; set; }

        public TodoItemRemoved(Guid itemId)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; private set; }
    }
}
