using Microsoft.AspNetCore.Identity;

namespace FlexyboxBlog.Models.Entities
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IdentityUser User { get; set; } = default!;
    }
}