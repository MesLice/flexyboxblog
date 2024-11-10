using FlexyboxShared.Models;
using FlexyboxShared.Models.Entities;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FlexyboxShared.Services
{
    public class BlogPostService
    {
        private readonly HttpClient _httpClient;

        public BlogPostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Fetch all blog posts
        public async Task<List<BlogPost>> GetAllPostsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<BlogPost>>("api/BlogPosts");
        }

        // Fetch a blog post by its ID
        public async Task<BlogPost> GetPostByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<BlogPost>($"api/BlogPosts/{id}");
        }

        // Create a new blog post
        public async Task CreatePostAsync(AddBlogPostDto newPost)
        {
            var response = await _httpClient.PostAsJsonAsync("api/BlogPosts", newPost);
            response.EnsureSuccessStatusCode();
        }

        // Update an existing blog post
        public async Task UpdatePostAsync(Guid id, UpdatePostDto updatedPost)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/BlogPosts/{id}", updatedPost);
            response.EnsureSuccessStatusCode();
        }

        // Delete a blog post
        public async Task DeletePostAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/BlogPosts/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}