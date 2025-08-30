using SQLite;
using System.Text.Json.Serialization;

namespace snfcofcBlzrwb.Models
{
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        [JsonIgnore] // Este campo es solo local, no viene del JSON
        public int LocalId { get; set; }

        [JsonPropertyName("objectId")]
        public string ObjectId { get; set; }

        [JsonPropertyName("Nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("Posicion")]
        public string Posicion { get; set; }

        [JsonPropertyName("Dorsal")]
        public int Dorsal { get; set; }

        [JsonPropertyName("ClaveSub")]
        public bool ClaveSub { get; set; }

        [JsonPropertyName("ClavePlus")]
        public bool ClavePlus { get; set; }

        [JsonPropertyName("Ranking")]
        public float Ranking { get; set; }
        [JsonPropertyName("FotoPlayer")]
        public ParseFile FotoPlayer { get; set; }

        [JsonPropertyName("IsSynced")]
        public bool IsSynced { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; } // opcional pero útil
    }

    public class ParseFile
    {
        [JsonPropertyName("__type")]
        public string Type { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

}