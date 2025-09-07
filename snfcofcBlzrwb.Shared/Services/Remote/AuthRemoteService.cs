
using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
namespace snfcofcBlzrwb.Shared.Services.Remote
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Web;
    using snfcofcBlzrwb.Shared.Models;

    public class AuthRemoteService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "https://parseapi.back4app.com";
        private const string ApplicationId = "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB";
        private const string RestApiKey = "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c";

        public AuthRemoteService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("X-Parse-Application-Id", ApplicationId);
            _client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", RestApiKey);
        }

        public async Task<(string sessionToken, string objectId, string email)> LoginAsync(string username, string password)
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

            return (sessionToken, objectId, email);
        }

        public async Task<List<string>> GetRolesAsync(string userObjectId, string sessionToken)
        {
            _client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sessionToken);

            var where = HttpUtility.UrlEncode(
                $"{{\"users\":{{\"__type\":\"Pointer\",\"className\":\"_User\",\"objectId\":\"{userObjectId}\"}}}}"
            );

            var url = $"{BaseUrl}/roles?where={where}";
            var response = await _client.GetAsync(url);
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
    }


}