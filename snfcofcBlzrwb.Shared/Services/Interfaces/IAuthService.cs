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
    public interface IAuthService
    {
        event Action<User> OnSessionChanged;

        Task<(string sessionToken, string objectId, string email)> ValidateUserAsync(string username, string password);
        Task<List<string>> ObtenerRolesUsuarioAsync(string userObjectId, string sessionToken);

        void SetSession(User user);
        bool IsAuthenticated();
        List<string> GetUserRoles();
        string GetSessionToken();
        User GetCurrentUser();
        void ClearSession();
    }
}
