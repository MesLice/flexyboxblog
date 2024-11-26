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

        public async Task<List<BlogPostDto>> GetAllPostsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<BlogPostDto>>("api/BlogPosts");
            }
            catch (HttpRequestException ex)
            {
                LogError("Error fetching all blog posts", ex);
                throw;
            }
        }

        public async Task<BlogPostDto> GetPostByIdAsync(Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<BlogPostDto>($"api/BlogPosts/{id}");
            }
            catch (HttpRequestException ex)
            {
                LogError($"Error fetching blog post with ID: {id}", ex);
                throw;
            }
        }

        public async Task CreatePostAsync(BlogPostDto newPost)
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

        public async Task UpdatePostAsync(Guid id, BlogPostDto updatedPost)
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