using FlexyboxBlog.Data;
using FlexyboxBlog.Models;
using FlexyboxBlog.Models.Entities;
using FlexyboxShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlexyboxBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllPosts()
        {
            return Ok(dbContext.BlogPosts.ToList());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetPostById(Guid id)
        {
            var result = dbContext.BlogPosts.Find(id);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreatePost(AddBlogPostDto addBlogPostDto)
        {
            var blogPost = new BlogPost()
            {
                Title = addBlogPostDto.Title,
                Content = addBlogPostDto.Content,
                CreatedAt = DateTime.Now
            };

            dbContext.BlogPosts.Add(blogPost);
            dbContext.SaveChanges();
            return Created();
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdatePost(Guid id, UpdatePostDto updatePostDto)
        {
            var blogPost = dbContext.BlogPosts.Find(id);
            if (blogPost is null)
            {
                return NotFound();
            }

            blogPost.Title = updatePostDto.Title;
            blogPost.Content = updatePostDto.Content;
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeletePost(Guid id)
        {
            var blogPost = dbContext.BlogPosts.Find(id);
            if (blogPost is null)
            {
                return NotFound();
            }

            dbContext.BlogPosts.Remove(blogPost);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
