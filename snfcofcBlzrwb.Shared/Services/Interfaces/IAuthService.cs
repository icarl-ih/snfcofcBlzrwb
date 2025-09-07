using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using snfcofcBlzrwb.Shared.Models;

namespace snfcofcBlzrwb.Shared.Services.Interfaces
{
    // IAuthService.cs
    public interface IAuthService
    {
        event Action<User> OnSessionChanged;

        // Login que ya tienes:
        Task<(string sessionToken, string objectId, string email)> ValidateUserAsync(string username, string password);

        // NUEVO: restaurar sesión al arrancar app
        Task InitializeAsync();

        // Ya existente:
        Task<List<string>> ObtenerRolesUsuarioAsync(string userObjectId, string sessionToken);

        // Estado de sesión
        void SetSession(User user);
        bool IsAuthenticated();          // puedes mantenerlo
        List<string> GetUserRoles();
        string GetSessionToken();
        User GetCurrentUser();
        void ClearSession();
    }
}
