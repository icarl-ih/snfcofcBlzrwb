using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Interfaces
{

    public interface IMatchService : IConnectivityAwareService
    {
        Task<List<MatchModel>> GetAllAsync();
        Task<MatchModel?> GetByIdAsync(string objectId);
        Task SaveAsync(MatchModel match);
        Task<List<MatchModel>> GetUnsyncedAsync();
        Task DeleteAsync(string objectId);
        Task CreateMatch(MatchModel match);
        Task<(string name, string url)> SubirFotoEquipoAsync(byte[] data, string nombreArchivo);

    }
}
