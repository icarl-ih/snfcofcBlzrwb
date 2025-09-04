using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Models
{
    public class User
    {
        [JsonPropertyName("objectId")]
        public string ObjectId { get; set; }        
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("sessionToken")]
        public string SessionToken { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();


    }
    public enum RolUser
    {
        Ninguno,
        SoloLectura,
        Editor,
        Admin
    }

    

}
