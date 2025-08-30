using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Remote
{
    public class EvaluationRemoteService : IEvaluationService
    {
        private readonly HttpClient _http;
        public bool IsOnline { get; private set; } = true;

        public EvaluationRemoteService(HttpClient http) => _http = http;
        public void SetConnectivity(bool isOnline) => IsOnline = isOnline;

        public async Task SaveAsync(PlayerEvaluation evaluation)
        {
            if (string.IsNullOrEmpty(evaluation.ObjectId))
                evaluation.ObjectId = Guid.NewGuid().ToString();

            evaluation.UpdatedAt = DateTime.UtcNow;
            evaluation.IsSynced = true;

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
                { "isSynced", true }
        };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("/classes/Evaluations", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<PlayerEvaluation>> GetAllAsync()
        {
            var response = await _http.GetAsync("/classes/Evaluations");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ParseResponse<PlayerEvaluation>>(json);
            return result?.Results ?? new List<PlayerEvaluation>();
        }

        public Task<PlayerEvaluation?> GetByIdAsync(string objectId) =>
            throw new NotImplementedException();

        public Task DeleteAsync(string objectId) =>
            _http.DeleteAsync($"/classes/Evaluations/{objectId}");

        public Task<List<PlayerEvaluation>> GetUnsyncedAsync() =>
            Task.FromResult(new List<PlayerEvaluation>());
    }
}
