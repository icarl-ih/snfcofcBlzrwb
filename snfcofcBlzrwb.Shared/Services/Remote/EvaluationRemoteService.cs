using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using snfcofcBlzrwb.Shared.Data;

namespace snfcofcBlzrwb.Shared.Services.Remote
{
    public class EvaluationRemoteService : IEvaluationService
    {
        private readonly HttpClient _http;
        public bool IsOnline { get; private set; } = true;
        private readonly IAuthService _authService;

        public EvaluationRemoteService(HttpClient http)
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
        public void SetConnectivity(bool isOnline) => IsOnline = isOnline;

        public async Task<List<PlayerEvaluation>> GetAllAsync()
        {
            var response = await _http.GetAsync("/classes/PlayerEvaluation");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ParseResponse<PlayerEvaluation>>(json);
            return result?.Results ?? new List<PlayerEvaluation>();
        }
        public async Task<PlayerEvaluation?> GetByIdAsync(string objectId)
        {
            var response = await _http.GetAsync($"/classes/PlayerEvaluation/{objectId}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var evaluation = JsonSerializer.Deserialize<PlayerEvaluation>(json);
            return evaluation;
        }
        public async Task SaveAsync(PlayerEvaluation evaluation)
        {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            var sesionToken = _authService.GetSessionToken();
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sesionToken);

            var payload = new Dictionary<string, object>
        {
            { "PlayerObjectId", evaluation.PlayerObjectId },
            { "MatchObjectId", evaluation.MatchObjectId },
            { "Comentarios", evaluation.Comentarios },
                {"FieldSkillScore",evaluation.FieldSkillScore },
                {"RespetaCompaneros",evaluation.RespetaCompaneros},
                {"RespetaTecnico", evaluation.RespetaTecnico},
                { "RespetaArbitro", evaluation.RespetaArbitro},
                {"Tarjetas", evaluation.Tarjetas},
                {"FaltasLenguaje", evaluation.FaltasLenguaje},
                {"Asistio", evaluation.Asistio},
                {"FuePuntual", evaluation.FuePuntual},
                {"MinutosAntes", evaluation.MinutosAntes},
                { "IsSynced", true }
        };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(
                $"https://parseapi.back4app.com/classes/PlayerEvaluation/{evaluation.ObjectId}",
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

        public async Task CreateEvaluationAsync(PlayerEvaluation evaluation)
        {
            int.TryParse(Guid.NewGuid().ToString(), out var id);
            evaluation.LocalId = id;
            var parseObject = new Dictionary<string, object>
            {
                {"LocalId",evaluation.LocalId },
                { "PlayerObjectId", evaluation.PlayerObjectId },
                { "MatchObjectId", evaluation.MatchObjectId },
                { "Comentarios", evaluation.Comentarios },
                {"FieldSkillScore",evaluation.FieldSkillScore },
                {"RespetaCompaneros",evaluation.RespetaCompaneros},
                {"RespetaTecnico", evaluation.RespetaTecnico},
                { "RespetaArbitro", evaluation.RespetaArbitro},
                {"Tarjetas", evaluation.Tarjetas},
                {"FaltasLenguaje", evaluation.FaltasLenguaje},
                {"Asistio", evaluation.Asistio},
                {"FuePuntual", evaluation.FuePuntual},
                {"MinutosAntes", evaluation.MinutosAntes},
                { "IsSynced", true }

            };

            
            var json = JsonSerializer.Serialize(parseObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("/classes/PlayerEvaluation", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string objectId)
        {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            var sesionToken = _authService.GetSessionToken();
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sesionToken);

            var response = await client.DeleteAsync($"https://parseapi.back4app.com/classes/PlayerEvaluation/{objectId}");
            response.EnsureSuccessStatusCode();
        }
        public Task<List<PlayerEvaluation>> GetUnsyncedAsync() =>
            Task.FromResult(new List<PlayerEvaluation>());
    }
}
