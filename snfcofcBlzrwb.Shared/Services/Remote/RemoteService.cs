using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Web;
using snfcofcBlzrwb.Shared.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;

public class RemoteService
{
    private readonly HttpClient _client;
    private User _currentUser;
    private bool _isOnline = true;

    private const string BaseUrl = "https://parseapi.back4app.com";
    private const string ApplicationId = "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB";
    private const string RestApiKey = "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c";

    public event Action<User> OnSessionChanged;

    public RemoteService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _client.DefaultRequestHeaders.Add("X-Parse-Application-Id", ApplicationId);
        _client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", RestApiKey);
    }

    public async Task<(string sessionToken, string objectId, string email)> ValidateUserAsync(string username, string password)
    {
        var url = $"{BaseUrl}/login?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";
        var response = await _client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Login failed: {content}");

        var json = JsonDocument.Parse(content);
        string sessionToken = json.RootElement.GetProperty("sessionToken").GetString();
        string objectId = json.RootElement.GetProperty("objectId").GetString();
        string email = json.RootElement.TryGetProperty("email", out var emailToken) && emailToken.ValueKind == JsonValueKind.String
            ? emailToken.GetString()
            : "";

        var roles = await ObtenerRolesUsuarioAsync(objectId, sessionToken);

        _currentUser = new User
        {
            ObjectId = objectId,
            Email = email,
            Username = username,
            SessionToken = sessionToken,
            Roles = roles
        };

        OnSessionChanged?.Invoke(_currentUser);

        return (sessionToken, objectId, email);
    }

    public async Task<List<string>> ObtenerRolesUsuarioAsync(string userObjectId, string sessionToken)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Parse-Application-Id", ApplicationId);
        client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", RestApiKey);
        client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sessionToken);

        var where = HttpUtility.UrlEncode(
            $"{{\"users\":{{\"__type\":\"Pointer\",\"className\":\"_User\",\"objectId\":\"{userObjectId}\"}}}}"
        );

        var url = $"{BaseUrl}/roles?where={where}";
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        var doc = JsonDocument.Parse(json);
        var roles = new List<string>();

        if (doc.RootElement.TryGetProperty("results", out var results) && results.ValueKind == JsonValueKind.Array)
        {
            foreach (var role in results.EnumerateArray())
            {
                roles.Add(role.GetProperty("name").GetString());
            }
        }

        return roles;
    }

    // IAuthService implementations
    public void SetSession(User user)
    {
        _currentUser = user;
        OnSessionChanged?.Invoke(user);
    }

    public bool IsAuthenticated() => _currentUser != null;

    public List<string> GetUserRoles() => _currentUser?.Roles ?? new List<string>();

    public string GetSessionToken() => _currentUser?.SessionToken;

    public User GetCurrentUser() => _currentUser;

    public void ClearSession()
    {
        _currentUser = null;
        OnSessionChanged?.Invoke(null);
    }

    // IConnectivityAwareService implementations
    public void SetConnectivity(bool isOnline)
    {
        _isOnline = isOnline;
    }

    public bool IsOnline => _isOnline;
}
