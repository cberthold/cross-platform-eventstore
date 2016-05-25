using Infrastructure.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.BoundedContext.Commands
{
    public class AddTodoItem : ICommand
    {
        public Guid TenantId { get; set; }
        public Guid ItemId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("completed")]
        public bool Completed { get; set; }
        public int ExpectedVersion { get; set; }
    }
}
