using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Models
{
    public class ToDoItem
    {
        public string Id { get; set; }

        public ToDoUser ToDoUser { get; set; }

        public string ToDoText { get; set; }

        public bool Complete { get; set; }
    }
}
