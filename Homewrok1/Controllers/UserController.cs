using Homework1.Data;
using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homewrok1.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(ReqResClient client) : ControllerBase
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
        return result is null ? BadRequest("Failed to create user.") : CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
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
        await _client.DeleteUser(id);
        return NoContent();
    }
}
