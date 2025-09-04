using snfcofcBlzrwb.Shared.Data;
using snfcofcBlzrwb.Shared.Models;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Remote
{
    public class RemoteService
    {
        private readonly AuthService _authService;
        private readonly HttpClient _client;
        private const string BaseUrl = "https://parseapi.back4app.com";
        private readonly string ApplicationId = "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB";
        private readonly string RestApiKey = "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c";
        //private readonly string SessionToken = "r:c9cccc509d533daf06bc928332d4670e";
        private readonly IAuthService _authService;

        public RemoteService(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }
        public RemoteService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("X-Parse-Application-Id", ApplicationId);
            _client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", RestApiKey);
            //_client.DefaultRequestHeaders.Add("X-Parse-Session-Token", SessionToken);
        }

        public async Task<(string sessiontoken, string objectId, string email)> ValidateUserAsync(string username, string password)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", ApplicationId);
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", RestApiKey);

            var url = $"https://parseapi.back4app.com/login?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";

            var response = await _client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Login failed: {content}");
            }

            var json = System.Text.Json.JsonDocument.Parse(content);
            string sessionToken = json.RootElement.GetProperty("sessionToken").GetString();
            string objectId = json.RootElement.GetProperty("objectId").GetString();
            string email = "";
            if (json.RootElement.TryGetProperty("email", out var emailToken) && emailToken.ValueKind == System.Text.Json.JsonValueKind.String)
                email = emailToken.GetString();

            var roles = await ObtenerRolesUsuarioAsync(objectId, sessionToken);


            User user = new User() { 
                ObjectId = objectId,
                Email = email,
                Username = username,
                SessionToken = sessionToken,
                Roles = roles
            };

            _authService.SetSession(user);





            return (sessionToken, objectId, email);
        }
        public async Task<List<string>> ObtenerRolesUsuarioAsync(string userObjectId, string sessionToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", ApplicationId);
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", RestApiKey);
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sessionToken);

            var where = System.Web.HttpUtility.UrlEncode(
                $"{{\"users\":{{\"__type\":\"Pointer\",\"className\":\"_User\",\"objectId\":\"{userObjectId}\"}}}}"
            );
            var url = $"https://parseapi.back4app.com/roles?where={where}";
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            var doc = System.Text.Json.JsonDocument.Parse(json);
            var roles = new List<string>();
            if (doc.RootElement.TryGetProperty("results", out var results) && results.ValueKind == System.Text.Json.JsonValueKind.Array)
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
