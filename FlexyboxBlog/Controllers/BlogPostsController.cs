using FlexyboxBlog.Data;
using FlexyboxBlog.Models.Entities;
using FlexyboxShared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlexyboxBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogPostsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _dbContext.BlogPosts.Include(p => p.User).ToListAsync();
            return Ok(posts);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var result = await _dbContext.BlogPosts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] AddBlogPostDto addBlogPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var blogPost = new BlogPost
            {
                Title = addBlogPostDto.Title,
                Content = addBlogPostDto.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId // Set the user ID for the blog post
            };

            _dbContext.BlogPosts.Add(blogPost);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostById), new { id = blogPost.Id }, blogPost);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto updatePostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blogPost = await _dbContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            // Ensure the logged-in user is the owner of the blog post
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (blogPost.UserId != userId)
            {
                return Forbid();
            }

            blogPost.Title = updatePostDto.Title;
            blogPost.Content = updatePostDto.Content;
            _dbContext.BlogPosts.Update(blogPost);
            await _dbContext.SaveChangesAsync();

            return NoContent(); // Returns 204 No Content to indicate success without body
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var blogPost = await _dbContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            // Ensure the logged-in user is the owner of the blog post
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (blogPost.UserId != userId)
            {
                return Forbid();
            }

            _dbContext.BlogPosts.Remove(blogPost);
            await _dbContext.SaveChangesAsync();
            return NoContent(); // Returns 204 No Content to indicate successful deletion
        }

        // Get Current User Endpoint
        [Authorize]
        [HttpGet("current-user")]
        public IActionResult GetCurrentUser()
        {
            var user = HttpContext.User;

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var username = user.Identity.Name;
            return Ok(new { Username = username });
        }

        // Get Posts of Current User Endpoint
        [Authorize]
        [HttpGet("my-posts")]
        public async Task<IActionResult> GetMyPosts()
        {
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            // Get posts belonging to the current user
            var posts = await _dbContext.BlogPosts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return Ok(posts);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBlogPosts(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query is required.");
            }

            var posts = await _dbContext.BlogPosts
                .Where(post => EF.Functions.Like(post.Title, $"%{query}%") ||
                               EF.Functions.Like(post.Content, $"%{query}%"))
                .OrderByDescending(post => post.CreatedAt)
                .ToListAsync();

            return Ok(posts);
        }

    }
}