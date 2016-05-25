using Infrastructure.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.BoundedContext.Data
{
    public class TodoListDTO : IEntity
    {
        public Guid Id { get; set; }
        [JsonProperty("todos")]
        public List<TodoItemDTO> Todos { get; set; }

        public TodoListDTO()
        {
            Todos = new List<TodoItemDTO>();
        }
    }
}
