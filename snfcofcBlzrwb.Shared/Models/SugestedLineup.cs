using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Models
{
    public class SuggestedLineup
    {
        [PrimaryKey, AutoIncrement]
        public int LocalId { get; set; }
        public string MatchObjectId { get; set; }
        public List<string> PlayerObjectIds { get; set; } // 11 jugadores
        public DateTime GeneratedAt { get; set; }
    }
}
