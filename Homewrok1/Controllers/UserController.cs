using Homewrok1.Data;
using Homewrok1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homewrok1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(APIContext context) : Controller
    {
        private readonly APIContext _context = context;

        [HttpGet]
        public IActionResult GetUser(int id)
        {
            var result = _context.Users.Find(id);

            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public IActionResult PostUser(User user)
        {
            if (user.Id == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                var userInDB = _context.Posts.Find(user.Id);

                if (userInDB is null)
                {
                    return NotFound();
                }
            }

            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var userDb = _context.Users.Find(id);

            if (userDb is null)
            {
                return NotFound();
            }

            userDb.FirstName = updatedUser.FirstName;
            userDb.LastName = updatedUser.LastName;
            userDb.Email = updatedUser.Email;
            userDb.Avatar = updatedUser.Avatar;

            _context.SaveChanges();
            
            return Ok(userDb);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userInDB = _context.Users.Find(id);

            if (userInDB is not null)
            {
                _context.Users.Remove(userInDB);
                _context.SaveChanges();
            }

            return NoContent();
        }
    }
}
