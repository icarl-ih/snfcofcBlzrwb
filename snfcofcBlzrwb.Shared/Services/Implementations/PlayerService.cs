using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Models;
using snfcofcBlzrwb.Shared.Services.Local;
using snfcofcBlzrwb.Shared.Services.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Implementations
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerService _local;
        private readonly IPlayerService _remote;
        public bool IsOnline { get; private set; }

        public PlayerService(IPlayerService local, IPlayerService remote)
        {
            _local = local;
            _remote = remote;
        }

        public void SetConnectivity(bool isOnline)
        {
            IsOnline = isOnline;
            _local.SetConnectivity(isOnline);
            _remote.SetConnectivity(isOnline);
        }

        public Task<List<Player>> GetAllAsync() =>
            IsOnline ? _remote.GetAllAsync() : _local.GetAllAsync();

        public Task<Player?> GetByIdAsync(string objectId) =>
            IsOnline ? _remote.GetByIdAsync(objectId) : _local.GetByIdAsync(objectId);

        public Task SaveAsync(Player player) =>
            IsOnline ? _remote.SaveAsync(player) : _local.SaveAsync(player);

        public Task DeleteAsync(string objectId) =>
            IsOnline ? _remote.DeleteAsync(objectId) : _local.DeleteAsync(objectId);

        public Task<List<Player>> GetUnsyncedAsync() =>
            _local.GetUnsyncedAsync(); // Solo el local tiene esta lógica
        public Task<(string name, string url)> SubirFotoUsuarioAsync(byte[] data, string nombreArchivo) =>
             _remote.SubirFotoUsuarioAsync(data, nombreArchivo);
        public Task CreatePlayerAsync(Player player) => _remote.CreatePlayerAsync(player);
    }
}
