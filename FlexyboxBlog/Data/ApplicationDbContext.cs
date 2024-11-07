using FlexyboxBlog.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlexyboxBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
