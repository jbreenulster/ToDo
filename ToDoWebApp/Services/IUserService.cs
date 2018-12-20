using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;

namespace ToDoApi.Services
{
    public interface IUserService
    {
        ToDoUser Authenticate(string username, string password);
        IEnumerable<ToDoUser> GetAll();
        ToDoUser GetById(int id);
        ToDoUser Create(ToDoUser user, string password);
        void Update(ToDoUser user);
        void Delete(string id);
    }
}
