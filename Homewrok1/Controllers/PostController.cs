using Homewrok1.Data;
using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homewrok1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(JsonPlaceholderClient client) : ControllerBase
    {
        private readonly JsonPlaceholderClient _client = client;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _client.GetPostById(id);
            return post == null ? NotFound() : Ok(post);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetPostByUserAndTitle([FromQuery] int userId, [FromQuery] string title)
        {
            var post = await _client.GetPostByUserAndTitle(userId, title);
            return post == null ? NotFound() : Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            var created = await _client.CreatePost(post);
            return created == null ? BadRequest("Failed to create post.") : CreatedAtAction(nameof(GetPostById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post updated)
        {
            var result = await _client.UpdatePost(id, updated);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var success = await _client.DeletePost(id);
            return success ? NoContent() : NotFound();
        }
    }
}
