using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Database;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoApiContext toDoApiContext;

        public ToDoController(ToDoApiContext toDoApiContext)
        {
            this.toDoApiContext = toDoApiContext;
        }
        
        // GET api/todo
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {

            var users = await toDoApiContext.ToDoUsers
                   .Include(u => u.ToDoItems)
                   .Where(u => u.Id == this.User.Identity.Name)
                   .ToArrayAsync();

            var response = users.Select(u => new
            {
                id = u.Id,
                firstName = u.FirstName,
                lastName = u.LastName,
                todos = u.ToDoItems.Select(p => new { p.Id, p.ToDoText })
            });

            return Ok(response);
        }

        // GET api/todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetAsync(string id)
        {
            var toDoItems = await toDoApiContext.ToDoItems.Include( u => u.ToDoUser).Where(x => x.Id == id).SingleOrDefaultAsync();

            if (toDoItems == null)
            {
                return NotFound("ToDo Item not found!");
            }

            var response = new
            {
                id = toDoItems.Id,
                item = toDoItems.ToDoText,
                userid = toDoItems.ToDoUser?.ConvertUserToUserProfile()
            };
            return Ok(response);
        }

        // POST api/todo
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string toDoText)
        {
            if (string.IsNullOrEmpty(toDoText))
            {
                throw new ArgumentException("Must supply ToDo Item");
            }

            var toDoItem = new ToDoItem
            {
                Id = Guid.NewGuid().ToString(),
                ToDoText = toDoText,
                ToDoUser = await this.toDoApiContext.ToDoUsers.Where(x => x.Id == "test").FirstOrDefaultAsync()
            };

        return NoContent();
        }

        // PUT api/todo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Must supply ToDo Item Id!");
            }

            var toDoItem = await toDoApiContext.ToDoItems.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (toDoItem == null)
            {
                return NotFound("ToDo Item not found!");
            }

            this.toDoApiContext.Remove(toDoItem);
            await this.toDoApiContext.SaveChangesAsync();

            return NoContent();

        }

        public string UserAccessToken()
        {
            string accessToken = User.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
            /*            var token = await Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.AuthenticateAsync(this..accessToken;
                        var authenticateInfo = await HttpContext.Authentication.GetAuthenticateInfoAsync("Bearer");
                        string accessToken = authenticateInfo.Properties.Items[".Token.access_token"];
              */
            return accessToken;

        }
    }
}
