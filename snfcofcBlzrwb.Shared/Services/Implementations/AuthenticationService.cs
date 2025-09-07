using snfcofcBlzrwb.Shared.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Services.Remote;
using Xamarin.Essentials;

namespace snfcofcBlzrwb.Shared.Services.Implementations
{
    public class AuthenticationService : IAuthService
    {
        private readonly AuthRemoteService _remote;
        private const string KeyToken = "parse_session_token";
        private const string KeyObjectId = "user_object_id";
        private const string KeyUsername = "user_username";
        private const string KeyEmail = "user_email";
        private const string KeyRoles = "user_roles_csv";

        public User _currentUser;
        private string _sessionToken;

        public event Action<User> OnSessionChanged;

        public AuthenticationService(AuthRemoteService remote)
        {
            _remote = remote;
        }

        public async Task InitializeAsync()
        {
            _sessionToken = await SecureStorage.GetAsync(KeyToken);

            if (!string.IsNullOrWhiteSpace(_sessionToken))
            {
                var objectId = Preferences.Get(KeyObjectId, null);
                var username = Preferences.Get(KeyUsername, null);
                var email = Preferences.Get(KeyEmail, null);
                var rolesCsv = Preferences.Get(KeyRoles, string.Empty);

                _currentUser = string.IsNullOrWhiteSpace(objectId) ? null : new User
                {
                    ObjectId = objectId,
                    Username = username,
                    Email = email,
                    Roles = string.IsNullOrWhiteSpace(rolesCsv)
                                ? new List<string>()
                                : rolesCsv.Split(',').Select(s => s.Trim())
                                          .Where(s => s.Length > 0).ToList()
                };
            }
            else
            {
                _currentUser = null;
            }

            OnSessionChanged?.Invoke(_currentUser);
        }

        public async Task<(string sessionToken, string objectId, string email)> ValidateUserAsync(string username, string password)
        {
            var (sessionToken, objectId, email) = await _remote.LoginAsync(username, password);

            _sessionToken = sessionToken;

            var roles = await _remote.GetRolesAsync(objectId, sessionToken);
            _currentUser = new User
            {
                ObjectId = objectId,
                Email = email,
                Username = username,
                Roles = roles
            };

            // Persistir
            await SecureStorage.SetAsync(KeyToken, sessionToken);
            Preferences.Set(KeyObjectId, objectId);
            Preferences.Set(KeyUsername, username ?? string.Empty);
            Preferences.Set(KeyEmail, email ?? string.Empty);
            Preferences.Set(KeyRoles, string.Join(",", roles ?? new()));

            OnSessionChanged?.Invoke(_currentUser);

            return (sessionToken, objectId, email);
        }

        public async Task<List<string>> ObtenerRolesUsuarioAsync(string userObjectId, string sessionToken)
            => await _remote.GetRolesAsync(userObjectId, sessionToken);

        public void SetSession(User user)
        {
            // Si llega un user sin token (p.ej. refresco desde API), preserva el token actual
            _currentUser = user ?? null;
            OnSessionChanged?.Invoke(_currentUser);
        }

        public bool IsAuthenticated() => !string.IsNullOrWhiteSpace(_sessionToken);

        public List<string> GetUserRoles() => _currentUser?.Roles ?? new List<string>();

        public string GetSessionToken() => _sessionToken;

        public User GetCurrentUser() => _currentUser;

        public void ClearSession()
        {
            // Limpia persistencia
            SecureStorage.Remove(KeyToken);
            Preferences.Remove(KeyObjectId);
            Preferences.Remove(KeyUsername);
            Preferences.Remove(KeyEmail);
            Preferences.Remove(KeyRoles);

            // Limpia memoria
            _sessionToken = null;
            _currentUser = null;
            OnSessionChanged?.Invoke(null);
        }
    }

}
