using FlexyboxShared.Models;
using FlexyboxShared.Models.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FlexyboxShared.Services
{
    public class BlogPostService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlogPostService> _logger;

        public BlogPostService(HttpClient httpClient, ILogger<BlogPostService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Fetch all blog posts
        public async Task<List<BlogPost>> GetAllPostsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<BlogPost>>("api/BlogPosts");
            }
            catch (HttpRequestException ex)
            {
                LogError("Error fetching all blog posts", ex);
                throw;
            }
        }

        // Fetch a blog post by its ID
        public async Task<BlogPost> GetPostByIdAsync(Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<BlogPost>($"api/BlogPosts/{id}");
            }
            catch (HttpRequestException ex)
            {
                LogError($"Error fetching blog post with ID: {id}", ex);
                throw;
            }
        }

        // Create a new blog post
        public async Task CreatePostAsync(AddBlogPostDto newPost)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/BlogPosts", newPost);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                LogError("Error creating new blog post", ex);
                throw;
            }
        }

        // Update an existing blog post
        public async Task UpdatePostAsync(Guid id, UpdatePostDto updatedPost)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/BlogPosts/{id}", updatedPost);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                LogError($"Error updating blog post with ID: {id}", ex);
                throw;
            }
        }

        // Delete a blog post
        public async Task DeletePostAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/BlogPosts/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                LogError($"Error deleting blog post with ID: {id}", ex);
                throw;
            }
        }

        // Helper method to log errors with inner exception details if available
        private void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, "{Message}: {ErrorMessage}", message, ex.Message);
            if (ex.InnerException != null)
            {
                _logger.LogError("Inner exception: {InnerMessage}", ex.InnerException.Message);
                _logger.LogError("Inner exception stack trace: {StackTrace}", ex.InnerException.StackTrace);
            }
        }
    }
}