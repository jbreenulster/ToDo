using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ToDoApi.Models;
using ToDoApi.Services;

namespace ToDoApi.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]ToDoUser userRecord)
        {
            var user = userService.Authenticate(userRecord.Username, userRecord.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("DeloitteToDoApiWebApp");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(string id, [FromBody]ToDoUser userRecord)
        {
            ToDoUser user = new ToDoUser
            {
                Id = id
            };

            try
            {
                // save 
                userService.Create(user, userRecord.Password);
                return Ok();
            }
            catch (AuthenticationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            var usersRecords = users.Select(x => x.ConvertUserToUserProfile());
            return Ok(usersRecords);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = userService.GetById(id);
            var userRecord = user.ConvertUserToUserProfile();
            return Ok(userRecord);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]ToDoUser userRecord)
        {

            try
            {
                // save 
                userService.Update(userRecord);
                return Ok();
            }
            catch (AuthenticationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            userService.Delete(id);
            return Ok();
        }
    }
}