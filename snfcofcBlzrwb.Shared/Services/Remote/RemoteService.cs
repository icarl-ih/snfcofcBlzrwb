using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Remote
{
    public class RemoteService
    {
        private readonly HttpClient _client;

        private readonly string ApplicationId = "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB";
        private readonly string RestApiKey = "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c";
        private readonly string SessionToken = "r:c9cccc509d533daf06bc928332d4670e";

        public RemoteService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("X-Parse-Application-Id", ApplicationId);
            _client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", RestApiKey);
            _client.DefaultRequestHeaders.Add("X-Parse-Session-Token", SessionToken);
        }

        public async Task<(string name, string url)> SubirFotoUsuarioAsync(byte[] data, string nombreArchivo)
        {
            var tipoMime = "image/jpeg";
            if (nombreArchivo.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                tipoMime = "image/png";
            else if (nombreArchivo.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || nombreArchivo.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                tipoMime = "image/jpeg";
            else if (nombreArchivo.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                tipoMime = "image/gif";

            var content = new ByteArrayContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue(tipoMime);

            var response = await _client.PostAsync($"https://parseapi.back4app.com/files/{nombreArchivo}", content);

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
    }


}
