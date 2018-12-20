using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Database
{
    public interface IToDoApiContext
    {

        DbSet<ToDoUser> ToDoUsers { get; set; }

        DbSet<ToDoItem> ToDoItems { get; set; }

        int SaveChanges();
    }
}

