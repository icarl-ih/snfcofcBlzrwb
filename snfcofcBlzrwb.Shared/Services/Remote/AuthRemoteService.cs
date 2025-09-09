using System.Net.Http;
using System.Text.Json;

namespace snfcofcBlzrwb.Shared.Services.Remote
{
    public class AuthRemoteService
    {
        private readonly IHttpClientFactory _factory;
        private const string ClientName = "ParseApi";

        public AuthRemoteService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<(string sessionToken, string objectId, string email)> LoginAsync(string username, string password)
        {
            var client = _factory.CreateClient(ClientName);

            var url = $"/login?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";
            using var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            using var json = JsonDocument.Parse(content);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Login failed ({(int)response.StatusCode}): {content}");

            var root = json.RootElement;
            var sessionToken = root.GetProperty("sessionToken").GetString() ?? "";
            var objectId = root.GetProperty("objectId").GetString() ?? "";
            var email = root.TryGetProperty("email", out var e) && e.ValueKind == JsonValueKind.String ? e.GetString() ?? "" : "";

            return (sessionToken, objectId, email);
        }

        public async Task<List<string>> GetRolesAsync(string userObjectId, string sessionToken)
        {
            var client = _factory.CreateClient(ClientName);

            var where = System.Web.HttpUtility.UrlEncode(
                $"{{\"users\":{{\"__type\":\"Pointer\",\"className\":\"_User\",\"objectId\":\"{userObjectId}\"}}}}"
            );

            using var req = new HttpRequestMessage(HttpMethod.Get, $"/roles?where={where}");
            req.Headers.Add("X-Parse-Session-Token", sessionToken); // <-- por request

            using var response = await client.SendAsync(req);
            var content = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(content);
            var roles = new List<string>();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"GetRoles failed ({(int)response.StatusCode}): {content}");

            if (doc.RootElement.TryGetProperty("results", out var results) && results.ValueKind == JsonValueKind.Array)
            {
                foreach (var role in results.EnumerateArray())
                    roles.Add(role.GetProperty("name").GetString() ?? "");
            }

            return roles;
        }
    }



}