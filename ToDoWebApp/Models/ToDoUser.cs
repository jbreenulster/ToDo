using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Models
{
    public class ToDoUser
    {
        public string Id { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<ToDoItem> ToDoItems { get; set; }

        public string Username => this.Id;
    }
}
