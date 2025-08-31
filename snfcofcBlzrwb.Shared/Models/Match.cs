using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Models
{
    public class MatchModel
    {
        [PrimaryKey, AutoIncrement]
        [JsonIgnore] // Este campo es solo local, no viene del JSON
        public int LocalId { get; set; }
        [JsonPropertyName("objectId")]
        public string ObjectId { get; set; }
        [JsonPropertyName("RivalObjectId")]
        public string RivalObjectId { get; set; }
        [JsonPropertyName("IsSynced")]
        public bool IsSynced { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("Rival")]
        public string Rival { get; set; }
        [JsonPropertyName("FotoRival")]
        public ParseFile FotoRival { get; set; }
        [JsonPropertyName("ClaveSub")]
        public bool ClaveSub { get; set; }

        [JsonPropertyName("ClavePlus")]
        public bool ClavePlus {  get; set; }
        [JsonPropertyName("JNo")]
            public int JNo { get; set; }
        [JsonPropertyName("EstatusMatchId")]
        public int EstatusMatchId { get; set; } = 0; // 3 = Ganado,  2 = Empate, 1 = Perdido y 0 = Pendiente de jugar
        [JsonPropertyName("Competencia")]
        public string Competencia { get; set; }
        [JsonPropertyName("FaseCompetencia")]
        public string FaseCompetencia { get; set; }
        [JsonPropertyName("FaGoles")]
        public int FaGoles { get; set; }
        [JsonPropertyName("CoGoles")]
        public int CoGoles { get; set; }


    }
}
