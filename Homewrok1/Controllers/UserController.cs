using Homework1.Data;
using Homework1.Models;
using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homewrok1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ReqResClient _client;
        private readonly ILogger<UsersController> _logger;
        private static readonly List<UserForm> _users = new();

        public UsersController(ReqResClient client, ILogger<UsersController> logger)
        {
            _client = client;
            _logger = logger;
        }

        [HttpGet("{username}")]
        public ActionResult<UserForm> GetUser(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                _logger.LogWarning($"User with username '{username}' not found.");
                return NotFound($"User with username '{username}' not found.");
            }
            _logger.LogInformation($"User with username '{username}' found.");
            return Ok(user);
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody] UserForm userForm)
        {
            if (_users.Any(u => u.Username == userForm.Username))
            {
                _logger.LogWarning($"User creation failed: The username '{userForm.Username}' already exists.");
                return BadRequest("The username is already taken. Please choose a different one.");
            }

            _users.Add(userForm);
            _logger.LogInformation($"User with username '{userForm.Username}' was successfully created.");
            return CreatedAtAction(nameof(GetUser), new { username = userForm.Username }, userForm);
        }

        [HttpPut("{username}")]
        public ActionResult UpdateUser(string username, [FromBody] UserForm updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                _logger.LogWarning($"User with username '{username}' not found for update.");
                return NotFound($"User with username '{username}' not found.");
            }

            user.Email = updatedUser.Email ?? user.Email;
            user.Password = updatedUser.Password ?? user.Password;
            _logger.LogInformation($"User with username '{username}' was successfully updated.");
            return Ok(user);
        }

        [HttpDelete("{username}")]
        public ActionResult DeleteUser(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                _logger.LogWarning($"User with username '{username}' not found for deletion.");
                return NotFound($"User with username '{username}' not found.");
            }

            _users.Remove(user);
            _logger.LogInformation($"User with username '{username}' was successfully deleted.");
            return NoContent();
        }
    }
}
