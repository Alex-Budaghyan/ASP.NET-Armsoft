using Homewrok1.Models;
using Microsoft.EntityFrameworkCore;

namespace Homewrok1.Data
{
    public class APIContext(DbContextOptions<APIContext> options) : DbContext(options)
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User>  Users { get; set; }
    }
}
