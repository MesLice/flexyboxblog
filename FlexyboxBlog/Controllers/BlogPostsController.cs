using FlexyboxBlog.Data;
using FlexyboxBlog.Models;
using FlexyboxBlog.Models.Entities;
using FlexyboxShared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var posts = await _dbContext.BlogPosts.ToListAsync();
            return Ok(posts);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var result = await _dbContext.BlogPosts.FindAsync(id);
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

            var blogPost = new BlogPost
            {
                Title = addBlogPostDto.Title,
                Content = addBlogPostDto.Content,
                CreatedAt = DateTime.UtcNow
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

            _dbContext.BlogPosts.Remove(blogPost);
            await _dbContext.SaveChangesAsync();
            return NoContent(); // Returns 204 No Content to indicate successful deletion
        }
    }
}