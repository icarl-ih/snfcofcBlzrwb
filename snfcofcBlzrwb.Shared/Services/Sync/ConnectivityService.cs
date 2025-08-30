using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; using System.Net.Http;
    using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Sync
{
   
        public class ConnectivityService
        {
            private readonly HttpClient _http;

            private const string Back4AppUrl = "https://parseapi.back4app.com/classes/Player?limit=1";
            private const string AppId = "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB";
            private const string ApiKey = "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c";

            public ConnectivityService(HttpClient http)
            {
                _http = http;
            }

            public async Task<bool> IsOnlineAsync()
            {
                try
                {
                    var response = await _http.GetAsync("https://www.google.com");
                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            }

            public async Task<bool> IsBack4AppAvailableAsync()
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, Back4AppUrl);
                    request.Headers.Add("X-Parse-Application-Id", AppId);
                    request.Headers.Add("X-Parse-REST-API-Key", ApiKey);

                    var response = await _http.SendAsync(request);
                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            }
        }
}
