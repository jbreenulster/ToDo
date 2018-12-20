using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Models;
using ToDoApi.Database;
using System.Security.Authentication;

namespace ToDoApi.Services
{
    public class UserService : IUserService
    {
        private IToDoApiContext _context;

        public UserService(IToDoApiContext context)
        {
            _context = context;
        }

        public ToDoUser Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.ToDoUsers.SingleOrDefault(x => x.Id == username && x.Password == password);

            // check if username exists with password
            if (user == null)
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<ToDoUser> GetAll()
        {
            return _context.ToDoUsers;
        }

        public ToDoUser GetById(int id)
        {
            return _context.ToDoUsers.Find(id);
        }

        public ToDoUser Create(ToDoUser user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            if (_context.ToDoUsers.Any(x => x.Id == user.Id))
                throw new AuthenticationException("Username \"" + user.Id + "\" is already taken");

            user.Password = password;

            _context.ToDoUsers.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(ToDoUser userParam)
        {
            var user = _context.ToDoUsers.Find(userParam.Id);

            if (user == null)
                throw new AuthenticationException("User not found");

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(userParam.Password))
            {
                user.Password = userParam.Password;
            }

            _context.ToDoUsers.Update(user);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var user = _context.ToDoUsers.Find(id);
            if (user != null)
            {
                _context.ToDoUsers.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
