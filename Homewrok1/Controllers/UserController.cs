using Homework1.Data;
using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController(ReqResClient client) : ControllerBase
    {
        private readonly ReqResClient _client = client;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _client.GetUser(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var result = await _client.CreateUser(user);
            return result is null ? BadRequest() : Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
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
