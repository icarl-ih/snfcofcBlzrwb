using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Models
{
    public class PlayerEvaluation
    {
        [PrimaryKey, AutoIncrement]
        [JsonIgnore] // Este campo es solo local, no viene del JSON
        public int LocalId { get; set; }
        [JsonPropertyName("objectId")]
        public string ObjectId { get; set; } // ID único si usas Back4App
        [JsonPropertyName("PlayerObjectId")]
        public string PlayerObjectId { get; set; }
        [JsonPropertyName("MatchObjectId")]
        public string MatchObjectId { get; set; }



        // Evaluación técnica
        [JsonPropertyName("FieldSkillScore")]
        public int FieldSkillScore { get; set; } // 1–10
        [JsonPropertyName("EstatusMatchId")]
        public int EstatusPartido { get; set; }
        

        // Disciplina
        [JsonPropertyName("RespetaCompaneros")]
        public bool RespetaCompaneros { get; set; }
        [JsonPropertyName("RespetaTecnico")]
        public bool RespetaTecnico { get; set; }
        [JsonPropertyName("RespetaArbitro")]
        public bool RespetaArbitro { get; set; }
        [JsonPropertyName("Tarjetas")]
        public int Tarjetas { get; set; } // 0 = limpio, 1 = amarilla, 2+ = roja/doble
        [JsonPropertyName("FaltasLenguaje")]
        public int FaltasLenguaje { get; set; } // 0, 1–2, 3+

        // Asistencia y puntualidad
        [JsonPropertyName("Asistio")]
        public bool Asistio { get; set; }
        [JsonPropertyName("FuePuntual")]
        public bool FuePuntual { get; set; }
        [JsonPropertyName("MinutosAntes")]
        public int MinutosAntes { get; set; } // solo si FuePuntual = true

        // Comentarios
        [JsonPropertyName("Comentarios")]
        public string Comentarios { get; set; }

        //Metadatos
        [JsonPropertyName("IsSynced")]
        public bool IsSynced { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

    }
}
