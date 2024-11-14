using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public ApiAuthenticationStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Make a call to get the current user claims
            var response = await _httpClient.GetAsync("/api/current-user");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserClaimsDto>();
                var identity = new ClaimsIdentity(user.Claims, "apiauth");
                var userPrincipal = new ClaimsPrincipal(identity);

                return new AuthenticationState(userPrincipal);
            }
        }
        catch (Exception)
        {
            // Handle exceptions
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void MarkUserAsAuthenticated(string username)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, username),
        }, "apiauth");

        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }
}

public class UserClaimsDto
{
    public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
}
