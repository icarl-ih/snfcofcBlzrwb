using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Data;
using snfcofcBlzrwb.Shared.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Remote
{
    public class TeamsRemoteService : ITeamService
    {
        private readonly AuthService _authService;
        private readonly HttpClient _http;
        public bool IsOnline { get; private set; } = true;

        public void SetConnectivity(bool isOnline) => IsOnline = isOnline;

        public TeamsRemoteService(HttpClient http)
        {
            _http = http;

            // Configurar BaseAddress si no está ya configurado
            if (_http.BaseAddress == null)
                _http.BaseAddress = new Uri("https://parseapi.back4app.com/");

            // Agregar headers si no están presentes
            if (!_http.DefaultRequestHeaders.Contains("X-Parse-Application-Id"))
                _http.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");

            if (!_http.DefaultRequestHeaders.Contains("X-Parse-REST-API-Key"))
                _http.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
        }

        public async Task<List<TeamModel>> GetAllAsync()
        {
            var response = await _http.GetAsync("classes/Teams");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ParseResponse<TeamModel>>(json);

            return result?.Results ?? new List<TeamModel>();
        }

        public async Task<TeamModel?> GetByIdAsync(string objectId)
        {
            var response = await _http.GetAsync($"/classes/Teams/{objectId}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var team = JsonSerializer.Deserialize<TeamModel>(json);
            return team;
        }

        public async Task SaveAsync(TeamModel team)
        {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", _authService.SessionToken);

            var parseObject = new Dictionary<string, object>
            {
                {"NombreEquipo",team.NombreEquipo },
                {"ClaveSub",team.ClaveSub},
                {"ClavePlus", team.ClavePlus},
                {"Competencia",team.Competencia}
                
            };

            if(team.FotoEscudo != null)
            {
                parseObject["FotoEscudo"] = new Dictionary<string, object>
                {
                    { "__type", "File" },
                    { "name", team.FotoEscudo.Name }
                };
            }
            var json = JsonSerializer.Serialize(parseObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(
                $"https://parseapi.back4app.com/classes/Teams/{team.ObjectId}",
                content);

            if (response.IsSuccessStatusCode)
            {
                // ¡Éxito!
                var respuestaString = await response.Content.ReadAsStringAsync();
                // Opcional: puedes extraer el objeto actualizado aquí
            }
            else
            {
                // Manejo de error
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error: {response.StatusCode} - {errorContent}");
            }
        }

        public async Task CreateTeamAsync(TeamModel team)
        {
            int.TryParse(Guid.NewGuid().ToString(), out var id);
            team.LocalId = id;
            var parseObject = new Dictionary<string, object>
            {
                {"LocalId",team.LocalId},
                {"NombreEquipo",team.NombreEquipo },
                {"ClaveSub",team.ClaveSub},
                {"ClavePlus", team.ClavePlus},
                {"Competencia",team.Competencia},
                {"FotoEscudo","" }

            };

            if (team.FotoEscudo != null)
            {
                parseObject["FotoEscudo"] = new Dictionary<string, object>
                {
                    { "__type", "File" },
                    { "name", team.FotoEscudo.Name }
                };
            }

            var json = JsonSerializer.Serialize(parseObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("/classes/Teams", content);
            response.EnsureSuccessStatusCode();
        }

        
        public async Task<(string name, string url)> SubirFotoTeamAsync(byte[] data, string nombreArchivo)
        {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", _authService.SessionToken);

            var tipoMime = "image/jpeg";
            if (nombreArchivo.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                tipoMime = "image/png";
            else if (nombreArchivo.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || nombreArchivo.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                tipoMime = "image/jpeg";
            else if (nombreArchivo.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                tipoMime = "image/gif";
            var content = new ByteArrayContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue(tipoMime);

            var response = await client.PostAsync($"https://parseapi.back4app.com/files/{nombreArchivo}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"No se pudo subir el archivo: {errorContent}");
            }

            var result = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(result);
            return (
                json.RootElement.GetProperty("name").GetString(),
                json.RootElement.GetProperty("url").GetString()
            );
        }

        public async Task DeleteAsync(string objectId)
        {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", _authService.SessionToken);

            var response = await client.DeleteAsync($"https://parseapi.back4app.com/classes/Teams/{objectId}");
            response.EnsureSuccessStatusCode();
        }

    }
}
