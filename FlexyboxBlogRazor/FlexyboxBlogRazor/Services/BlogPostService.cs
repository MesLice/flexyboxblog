using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FlexyboxShared.Models;
using FlexyboxShared.Models.Entities;

public class BlogPostService
{
    private readonly HttpClient _httpClient;

    public BlogPostService(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("API");
    }

    public async Task<List<BlogPost>> GetAllPostsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<BlogPost>>("api/BlogPosts");
    }

    public async Task<BlogPost> GetPostByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<BlogPost>($"api/BlogPosts/{id}");
    }

    public async Task CreatePostAsync(AddBlogPostDto newPost)
    {
        var response = await _httpClient.PostAsJsonAsync("api/BlogPosts", newPost);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePostAsync(Guid id, UpdatePostDto updatedPost)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/BlogPosts/{id}", updatedPost);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePostAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/BlogPosts/{id}");
        response.EnsureSuccessStatusCode();
    }
}