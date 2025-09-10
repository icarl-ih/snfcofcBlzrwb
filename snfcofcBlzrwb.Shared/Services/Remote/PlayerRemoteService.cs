using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Models;
using snfcofcBlzrwb.Shared.Services.Implementations;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace snfcofcBlzrwb.Shared.Services.Remote
{
    public class PlayerRemoteService : IPlayerService
    {
        private readonly IAuthService _auth;
        private readonly HttpClient _http;
        public bool IsOnline { get; private set; } = true;

        public void SetConnectivity(bool isOnline) => IsOnline = isOnline;

        public PlayerRemoteService(HttpClient http,IAuthService auth)
        {
            _http = http;
            _auth = auth;
            // Configurar BaseAddress si no está ya configurado
            if (_http.BaseAddress == null)
                _http.BaseAddress = new Uri("https://parseapi.back4app.com/");

            // Agregar headers si no están presentes
            if (!_http.DefaultRequestHeaders.Contains("X-Parse-Application-Id"))
                _http.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");

            if (!_http.DefaultRequestHeaders.Contains("X-Parse-REST-API-Key"))
                _http.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");

            // Si usas autenticación por sesión:
             
        }

        public async Task<List<Player>> GetAllAsync()
        {
            var response = await _http.GetAsync("classes/Players");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ParseResponse<Player>>(json);

            return result?.Results ?? new List<Player>();
        }


        public async Task<Player?> GetByIdAsync(string objectId)
        {
            var response = await _http.GetAsync($"/classes/Players/{objectId}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var player = JsonSerializer.Deserialize<Player>(json);
            return player;
        }

        public async Task SaveAsync(Player player)
        {
            try
            {

                //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
                client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
                var sesionToken = _auth.GetSessionToken();
                client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sesionToken);


                var parseObject = new Dictionary<string, object>
            {
                { "Nombre", player.Nombre },
                { "Posicion", player.Posicion },
                { "ClavePlus", player.ClavePlus },
                { "ClaveSub", player.ClaveSub },
                { "Dorsal", player.Dorsal },
                { "Ranking", player.Ranking },
                { "IsSynced", true },{"JugadosMatch",player.PartidosJugados},{"JugadosWin", player.PartidosGanados},{"JugadosDraw", player.PartidosEmpate},{"JugadosDefeat",player.PartidosPerdidos}
            };

                if (player.FotoPlayer != null)
                {
                    parseObject["FotoPlayer"] = new Dictionary<string, object>
                {
                    { "__type", "File" },
                    { "name", player.FotoPlayer.Name }
                };
                }

                var json = JsonSerializer.Serialize(parseObject);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(
                    $"https://parseapi.back4app.com/classes/Players/{player.ObjectId}",
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
            }catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception($"Excepción al guardar el jugador: {ex.Message}", ex);
            }
            }

        public async Task CreatePlayerAsync(Player player)
        {
            int.TryParse(Guid.NewGuid().ToString(), out var id);
            player.LocalId = id;
            var parseObject = new Dictionary<string, object>
            {
                {"LocalId",player.LocalId },
                { "Nombre", player.Nombre },
                { "Posicion", player.Posicion },
                { "ClavePlus", player.ClavePlus },
                { "ClaveSub", player.ClaveSub },
                { "Dorsal", player.Dorsal },
                { "Ranking", player.Ranking },
                { "IsSynced", true },
                {"FotoPlayer","" },{"JugadosMatch",player.PartidosJugados},{"JugadosWin", player.PartidosGanados},{"JugadosDraw", player.PartidosEmpate},{"JugadosDefeat",player.PartidosPerdidos}

            };

            if (player.FotoPlayer != null)
            {
                parseObject["FotoPlayer"] = new Dictionary<string, object>
                {
                    { "__type", "File" },
                    { "name", player.FotoPlayer.Name }
                };
            }
            var sessiontoken = _auth.GetSessionToken();
            _http.DefaultRequestHeaders.Add("X-Parse-Session-Token", sessiontoken);
            var json = JsonSerializer.Serialize(parseObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("/classes/Players", content);
            response.EnsureSuccessStatusCode();
        }


        public async Task<(string name, string url)> SubirFotoUsuarioAsync(byte[] data, string nombreArchivo)
        {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            var sesionToken = _auth
                .GetSessionToken();
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

        public async Task DeleteAsync(string objectId)
        {
            //string sessionToken = "r:c9cccc509d533daf06bc928332d4670e";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
            client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
            var sesionToken = _auth.GetSessionToken();
            client.DefaultRequestHeaders.Add("X-Parse-Session-Token", sesionToken);

            var response = await client.DeleteAsync($"https://parseapi.back4app.com/classes/Players/{objectId}");
            response.EnsureSuccessStatusCode();
        }


        public Task<List<Player>> GetUnsyncedAsync()
            => Task.FromResult(new List<Player>()); // No aplica en remoto
    }
}
