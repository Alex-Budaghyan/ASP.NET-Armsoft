using Homework1;
using Homework1.Models;
using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homewrok1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController(ReqResClient client, ILogger<UserController> logger) : ControllerBase
    {
        private readonly ReqResClient _client = client;
        private readonly ILogger<UserController> _logger = logger;

        private static readonly List<UserForm> _users = [];

        [HttpPost]
        public IActionResult SaveUser(UserForm user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                return BadRequest(new { message = "A user with that username already exists." });

            _users.Add(user);

            _logger.LogInformation("User '{Username}' was created successfully at {Time}.", user.Username, DateTime.UtcNow);

            return Ok(new { message = "User saved successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _client.GetUser(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            var result = await _client.CreateUser(user);
            return result is null ? BadRequest() : Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            var result = await _client.UpdateUser(id, updatedUser);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _client.DeleteUser(id);
            return success ? NoContent() : NotFound();
        }
    }
}
