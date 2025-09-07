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
        public string ObjectId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        // No dependas de que Parse te lo devuelva luego: lo gestionamos nosotros
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
