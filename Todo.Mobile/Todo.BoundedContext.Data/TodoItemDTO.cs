using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.BoundedContext.Data
{
    public class TodoItemDTO
    {
        [JsonProperty("id")]
        public Guid ItemId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("completed")]
        public bool Completed { get; set; }
    }
}
