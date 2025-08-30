using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Models
{
    public class ParseResponse<T>
    {
        [JsonPropertyName("results")]
        public List<T> Results { get; set; } = new();
    }
}
