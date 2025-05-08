using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homewrok1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController(JsonPlaceholderClient client) : ControllerBase
    {
        private readonly JsonPlaceholderClient _client = client;

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPostById(int id)
        {
            var post = await _client.GetPostById(id);
            return post == null ? NotFound() : Ok(post);
        }

        [HttpGet("search")]
        public async Task<ActionResult> GetPostByUserAndTitle([FromQuery] int userId, [FromQuery] string title)
        {
            var post = await _client.GetPostByUserAndTitle(userId, title);
            return post == null ? NotFound() : Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] Post post)
        {
            var created = await _client.CreatePost(post);
            return created is null ? BadRequest("Failed to create post.") : CreatedAtAction(nameof(GetPostById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost(int id, [FromBody] Post updated)
        {
            var result = await _client.UpdatePost(id, updated);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
           await _client.DeletePost(id);
            return NoContent();
        }
    }
}
