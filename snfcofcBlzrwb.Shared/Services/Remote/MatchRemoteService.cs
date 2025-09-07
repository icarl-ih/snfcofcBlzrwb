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
    public class MatchRemoteService : IMatchService
    {
        private readonly IAuthService _auth;
        private readonly HttpClient _http;
        private readonly AppSettings _appSettings;
        public bool IsOnline { get; private set; } = true;

        public MatchRemoteService(HttpClient http, IAuthService auth)
        {
            _http = http;
            _auth = auth;
            if (_http.BaseAddress == null)
                _http.BaseAddress = new Uri("https://parseapi.back4app.com/");
            if (!_http.DefaultRequestHeaders.Contains("X-Parse-Application-Id"))
                _http.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");

            if (!_http.DefaultRequestHeaders.Contains("X-Parse-REST-API-Key"))
                _http.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");

        }
        public void SetConnectivity(bool isOnline) => IsOnline = isOnline;

        public async Task<List<MatchModel>> GetAllAsync()
        {
            //_http.BaseAddress = new Uri("https://parseapi.back4app.com/");
            //_http.DefaultRequestHeaders.Add("X-Parse-Application-Id", _appSettings.ApplicationId);
            //_http.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", _appSettings.RestApiKey);

            var response = await _http.GetAsync("/classes/Match");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ParseResponse<MatchModel>>(json);
            return result?.Results ?? new List<MatchModel>();
        }

        public async Task<MatchModel?> GetByIdAsync(string objectId)
        {
            var response = await _http.GetAsync($"/classes/Match/{objectId}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var match = JsonSerializer.Deserialize<MatchModel>(json);
            return match;
        }
        public async Task SaveAsync(MatchModel match)
        {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            var sesionToken = _auth.GetSessionToken();
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sesionToken);

            var parseObject = new Dictionary<string, object>
            {
                
                {"EstatusMatchId",match.EstatusMatchId },
                
                {"FaGoles",match.FaGoles },{"CoGoles",match.CoGoles}
            
            };
            

            var json = JsonSerializer.Serialize(parseObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PutAsync(
                           $"https://parseapi.back4app.com/classes/Match/{match.ObjectId}",
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

        public async Task CreateMatch(MatchModel match)
        {
            int.TryParse(Guid.NewGuid().ToString(), out var id);
            match.LocalId = id;
            
            var parseObject = new Dictionary<string, object>
            {
                {"LocalId",match.LocalId },
                {"RivalObjectId",match.RivalObjectId },
                { "IsSynced", true },
                { "Rival" , match.Rival },
                {"FotoRival","" },
                {"ClaveSub",match.ClaveSub },
                {"ClavePlus",match.ClavePlus },
                {"JNo",match.JNo },
                {"EstatusMatchId",match.EstatusMatchId },
                {"Competencia",match.Competencia },
                {"FaseCompetencia",match.FaseCompetencia},
                {"FaGoles",match.FaGoles },{"CoGoles",match.CoGoles}

            };
            if (match.FotoRival != null)
            {
                parseObject["FotoRival"] = new Dictionary<string, object>
                {
                    {"__type", "File" },
                    { "name", match.FotoRival.Name }
                };
            }
            var json = JsonSerializer.Serialize(parseObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("/classes/Match", content);
            response.EnsureSuccessStatusCode();
        }

       

        public async Task<(string name, string url)> SubirFotoEquipoAsync(byte[] data, string nombreArchivo)
        {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            var sesionToken = _auth.GetSessionToken();
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sesionToken);

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
        public async Task DeleteAsync(string objectId) {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            var sesionToken = _auth.GetSessionToken();
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sesionToken);

            var response = await client.DeleteAsync($"https://parseapi.back4app.com/classes/Match/{objectId}");
            response.EnsureSuccessStatusCode();
        }


        
        

        public Task<List<MatchModel>> GetUnsyncedAsync() =>
            Task.FromResult(new List<MatchModel>());
    }
}
