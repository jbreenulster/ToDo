using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Models
{
    public static class ToDoUserHelper
    {
        public static ToDoUserProfile ConvertUserToUserProfile(this ToDoUser user)
        {
            return new ToDoUserProfile
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
        public static ToDoUser SaveClone(this ToDoUser user)
        {
            return new ToDoUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password
            };
        }
    }
}
