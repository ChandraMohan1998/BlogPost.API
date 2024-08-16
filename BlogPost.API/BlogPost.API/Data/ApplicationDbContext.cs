using Microsoft.EntityFrameworkCore;
using BlogPost.API.Model.Domain;

namespace BlogPost.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BloggerPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
