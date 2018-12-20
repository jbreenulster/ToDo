using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Database
{
    public class ToDoApiContext : DbContext, IToDoApiContext
    {
        public ToDoApiContext(DbContextOptions<ToDoApiContext> options)
                    : base(options)
        {
        }

        public DbSet<ToDoUser> ToDoUsers { get; set; }

        public DbSet<ToDoItem> ToDoItems { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}

