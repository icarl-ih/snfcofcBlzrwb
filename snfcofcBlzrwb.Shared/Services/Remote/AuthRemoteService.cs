#region ensamblado snfcofcBlzrwb.Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\carlos.ibarra\source\repos\snfcofcBlzrwb\snfcofcBlzrwb.Shared\obj\Debug\net9.0\ref\snfcofcBlzrwb.Shared.dll
#endregion

#nullable enable

using snfcofcBlzrwb.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
namespace snfcofcBlzrwb.Shared.Services.Remote
{
    public class AuthRemoteService : IAuthService
    {
        public string SessionToken { get; private set; }
        public string Username { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(SessionToken);

        private User? _currentUser;

        public event Action? OnSessionChanged;

        public task SetSession(User user)
        {
            _currentUser = user;
            OnSessionChanged?.Invoke();
        }

        public User? GetCurrentUser()
        {
            return _currentUser;
        }

        public bool IsAuthenticated()
        {
            return _currentUser != null && !string.IsNullOrEmpty(_currentUser.SessionToken);
        }

        public void ClearSession()
        {
            _currentUser = null;
            OnSessionChanged?.Invoke();
        }

        public string? GetSessionToken()
        {
            return _currentUser?.SessionToken;
        }

        public List<string> GetUserRoles()
        {
            return _currentUser?.Roles ?? new List<string>();
        }
    }
}