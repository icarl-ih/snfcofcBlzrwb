using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Interfaces
{
    public interface IAuthService : IConnectivityAwareService
    {
        // ✅ Guarda la sesión actual del usuario
        void SetSession(User user);

        // ✅ Obtiene el usuario actualmente autenticado
        User? GetCurrentUser();

        // ✅ Verifica si hay sesión activa
        bool IsAuthenticated();

        // ✅ Elimina la sesión actual
        void ClearSession();

        // ✅ Retorna el token de sesión si existe
        string? GetSessionToken();

        // ✅ Retorna los roles del usuario actual
        List<string> GetUserRoles();

        // ✅ Evento opcional para notificar cambios de sesión (útil en Blazor)
        event Action? OnSessionChanged;
    }
}
