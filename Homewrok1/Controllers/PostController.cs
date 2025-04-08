using Homewrok1.Data;
using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homewrok1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(APIContext context) : Controller
    {
        private readonly APIContext  _context = context; 

        [HttpGet]
        public IActionResult GetPost(int id, string title)
        {
            var result = _context.Posts.FirstOrDefault(p => p.Id == id && p.Title == title);
            
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetPost(int id)
        {
            var result = _context.Posts.Find(id);

            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public IActionResult CreatePost(Post post)
        {
            if (post.Id == 0)
            {
                _context.Posts.Add(post);
            }
            else
            {
                var postInDB = _context.Posts.Find(post.Id);

                if (postInDB is null)
                {
                    return new JsonResult(NotFound());
                }
            }

            _context.SaveChanges();

            return Ok(post);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, Post updatedPost)
        {
            var postDb = _context.Posts.Find(id);
            if (postDb is null)
                return new JsonResult(NotFound());

            postDb.UserID = updatedPost.UserID;
            postDb.Title = updatedPost.Title;
            postDb.Body = updatedPost.Body;

            _context.SaveChanges();
            return Ok(postDb);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            var postDb = _context.Posts.Find(id);
            if (postDb is not null)
            {
                _context.Posts.Remove(postDb);
                _context.SaveChanges();
            }

            return NoContent();
        }

    }
}
