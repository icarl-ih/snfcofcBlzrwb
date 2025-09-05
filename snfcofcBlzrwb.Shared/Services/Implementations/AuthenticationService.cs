using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Services.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace snfcofcBlzrwb.Shared.Services.Implementations
{
    public class AuthenticationService : IAuthService
    {
        private readonly AuthRemoteService _remote;
        private User _currentUser;

        public event Action<User> OnSessionChanged;

        public AuthenticationService(AuthRemoteService remote)
        {
            _remote = remote;
        }

        public async Task<(string sessionToken, string objectId, string email)> ValidateUserAsync(string username, string password)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"[PERF] Iniciando el Login {DateTime.Now:HH:mm:ss.ffff}");

            var (sessionToken, objectId, email) = await _remote.LoginAsync(username, password);
            Console.WriteLine($"[PERF] LoginAsync tardó {sw.ElapsedMilliseconds} ms");

            Console.WriteLine($"[PERF] Obteniendo Roles {DateTime.Now:HH:mm:ss.ffff}");

            var roles = await _remote.GetRolesAsync(objectId, sessionToken);
            Console.WriteLine($"[PERF] GetRolesAsync {sw.ElapsedMilliseconds} ms");

            Console.WriteLine($"[PERF] Asignando nuevo usuario {DateTime.Now:HH:mm:ss.ffff}");
            _currentUser = new User
            {
                ObjectId = objectId,
                Email = email,
                Username = username,
                SessionToken = sessionToken,
                Roles = roles
            };
            Console.WriteLine($"[PERF] Asignando sesion: {DateTime.Now:HH:mm:ss.ffff}");

            SetSession(_currentUser);
            OnSessionChanged?.Invoke(_currentUser);

            Console.WriteLine($"[PERF] Terminsndo y retornando: {DateTime.Now:HH:mm:ss.ffff}");
            return (sessionToken, objectId, email);
        }

        public async Task<List<string>> ObtenerRolesUsuarioAsync(string userObjectId, string sessionToken)
        {
            return await _remote.GetRolesAsync(userObjectId, sessionToken);
        }

        public void SetSession(User user)
        {
            _currentUser = user;
            OnSessionChanged?.Invoke(user);
        }

        public bool IsAuthenticated() => _currentUser != null;

        public List<string> GetUserRoles() => _currentUser?.Roles ?? new List<string>();

        public string GetSessionToken() => _currentUser?.SessionToken;

        public User GetCurrentUser() => _currentUser;

        public void ClearSession()
        {
            _currentUser = null;
            OnSessionChanged?.Invoke(null);
        }
    }

}
