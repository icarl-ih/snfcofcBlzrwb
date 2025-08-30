using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Local
{
    public class PlayerLocalService : IPlayerService
    {
        private readonly SQLiteAsyncConnection _db;
        public bool IsOnline { get; private set; } = false;

        public void SetConnectivity(bool isOnline) => IsOnline = isOnline;

        public PlayerLocalService(SQLiteAsyncConnection db) => _db = db;

        public async Task<List<Player>> GetAllAsync() =>
            await _db.Table<Player>().ToListAsync();

        public async Task<Player?> GetByIdAsync(string objectId) =>
            await _db.Table<Player>().FirstOrDefaultAsync(p => p.ObjectId == objectId);

        public async Task SaveAsync(Player player)
        {
            if (string.IsNullOrEmpty(player.ObjectId))
                player.ObjectId = Guid.NewGuid().ToString();

            player.UpdatedAt = DateTime.UtcNow;
            player.IsSynced = false;
            await _db.InsertOrReplaceAsync(player);
        }

        public async Task DeleteAsync(string objectId)
        {
            var player = await GetByIdAsync(objectId);
            if (player != null)
                await _db.DeleteAsync(player);
        }

        public async Task<List<Player>> GetUnsyncedAsync()
        {
            var all = await GetAllAsync();
            return all.Where(p => !p.IsSynced).ToList();
        }
        public async Task<(string name, string url)> SubirFotoUsuarioAsync(byte[] data, string nombreArchivo) { return("",""); }

        public async Task CreatePlayerAsync(Player player) { }
    }
}
