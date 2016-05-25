using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.BoundedContext.Domain
{
    public class TodoItemEntity
    {
        public Guid ItemId { get; private set; }
        public string Title { get; private set; }
        public int Order { get; private set; }
        public bool Completed { get; private set; }

        public TodoItemEntity(Guid itemId, string title, int order, bool completed)
        {
            ItemId = itemId;
            Order = order;
            Update(title, completed);
        }

        public void Update(string title, bool completed)
        {
            Title = title;
            Completed = completed;
        }
    }
}
