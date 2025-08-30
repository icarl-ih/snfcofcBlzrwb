using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Interfaces
{
    public interface IPlayerService : IConnectivityAwareService
    {
        Task<List<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(string objectId);
        Task SaveAsync(Player player);
        Task<List<Player>> GetUnsyncedAsync();
        Task DeleteAsync(string objectId);
        Task CreatePlayerAsync(Player player);
        Task<(string name, string url)> SubirFotoUsuarioAsync(byte[] data, string nombreArchivo);
    }
}
