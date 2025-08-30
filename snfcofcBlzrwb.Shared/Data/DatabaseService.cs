using snfcofcBlzrwb.Models;
using SQLite;

namespace snfcofcBlzrwb.Shared.Data
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _database;

        public static async Task InitAsync()
        {
            if (_database != null) return;

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"ihsolutionsdb.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<Player>();
            await _database.CreateTableAsync<MatchModel>();
            await _database.CreateTableAsync<PlayerEvaluation>();
        }

        public static SQLiteAsyncConnection GetConnection() => _database;
    }

}
