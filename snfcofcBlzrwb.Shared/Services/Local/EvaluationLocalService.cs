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
    public class EvaluationLocalService : IEvaluationService
    {
        private readonly SQLiteAsyncConnection _db;
        public bool IsOnline { get; private set; } = false;

        public EvaluationLocalService(SQLiteAsyncConnection db) => _db = db;
        public void SetConnectivity(bool isOnline) => IsOnline = isOnline;

        public Task<List<PlayerEvaluation>> GetAllAsync() =>
            _db.Table<PlayerEvaluation>().ToListAsync();

        public Task<PlayerEvaluation?> GetByIdAsync(string objectId) =>
            _db.Table<PlayerEvaluation>().FirstOrDefaultAsync(e => e.ObjectId == objectId);

        public async Task SaveAsync(PlayerEvaluation evaluation)
        {
            if (string.IsNullOrEmpty(evaluation.ObjectId))
                evaluation.ObjectId = Guid.NewGuid().ToString();

            evaluation.UpdatedAt = DateTime.UtcNow;
            evaluation.IsSynced = false;
            await _db.InsertOrReplaceAsync(evaluation);
        }

        public async Task DeleteAsync(string objectId)
        {
            var eval = await GetByIdAsync(objectId);
            if (eval != null)
                await _db.DeleteAsync(eval);
        }

        public async Task<List<PlayerEvaluation>> GetUnsyncedAsync()
        {
            var all = await GetAllAsync();
            return all.Where(e => !e.IsSynced).ToList();
        }
        public async Task CreateEvaluationAsync(PlayerEvaluation evaluation) { }
        public Task<List<string>> GetEvaluadosIds(string objectId)
        {
            // Devuelve una lista vacía por defecto para cumplir con la firma y evitar CS0161.
            return Task.FromResult(new List<string>());
        }

    }

}
