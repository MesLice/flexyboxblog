using Microsoft.AspNetCore.Identity;

namespace FlexyboxBlog.Models.Entities
{
    public class BlogPost
    {
        public Guid Id { get; set; }

        public required string Title { get; set; }
        public required string Content { get; set; }

        // Created date
        public DateTime CreatedAt { get; set; }

        // Foreign key for the user who created the post
        public string UserId { get; set; } = string.Empty;

        // Navigation property for the associated user
        public IdentityUser User { get; set; } = default!;
    }
}