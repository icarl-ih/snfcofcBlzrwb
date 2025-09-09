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
    public class MatchLocalService : IMatchService
    {
        private readonly SQLiteAsyncConnection _db;
        public bool IsOnline { get; private set; } = false;

        public MatchLocalService(SQLiteAsyncConnection db) => _db = db;
        public void SetConnectivity(bool isOnline) => IsOnline = isOnline;

        public Task<List<MatchModel>> GetAllAsync() => _db.Table<MatchModel>().ToListAsync();
        public Task<MatchModel?> GetByIdAsync(string objectId) =>
            _db.Table<MatchModel>().FirstOrDefaultAsync(m => m.ObjectId == objectId);

        public async Task SaveAsync(MatchModel match)
        {
            if (string.IsNullOrEmpty(match.ObjectId))
                match.ObjectId = Guid.NewGuid().ToString();

            match.UpdatedAt = DateTime.UtcNow;
            match.IsSynced = false;
            await _db.InsertOrReplaceAsync(match);
        }

        public async Task DeleteAsync(string objectId)
        {
            var match = await GetByIdAsync(objectId);
            if (match != null)
                await _db.DeleteAsync(match);
        }

        public async Task<List<MatchModel>> GetUnsyncedAsync()
        {
            var all = await GetAllAsync();
            return all.Where(m => !m.IsSynced).ToList();
        }
        public async Task<(string name, string url)> SubirFotoEquipoAsync(byte[] data, string nombreArchivo) { return ("", ""); }

        public async Task CreateMatch(MatchModel match) { }
        public async Task<List<MatchModel>> GetUnEvaluatedMatches(List<string> ids, Player player) { return new List<MatchModel>(); }
    }
}
