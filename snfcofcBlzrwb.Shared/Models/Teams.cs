using snfcofcBlzrwb.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Models
{
    public class TeamModel
    {
        [PrimaryKey, AutoIncrement]
        [JsonIgnore] // Este campo es solo local, no viene del JSON
        public int LocalId { get; set; }
        [JsonPropertyName("objectId")]
        public string ObjectId { get; set; }
        [JsonPropertyName("NombreEquipo")]
        public string NombreEquipo { get; set; }
        [JsonPropertyName("ClaveSub")]
        public bool ClaveSub { get; set; }
        [JsonPropertyName("ClavePlus")]
        public bool ClavePlus { get; set; }
        [JsonPropertyName("Competencia")]
        public string Competencia { get; set; }
        [JsonPropertyName("FotoEscudo")]
        public ParseFile FotoEscudo { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
