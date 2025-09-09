using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Interfaces
{
    public interface IEvaluationService : IConnectivityAwareService
    {
        Task<List<PlayerEvaluation>> GetAllAsync();
        Task<PlayerEvaluation?> GetByIdAsync(string objectId);
        Task SaveAsync(PlayerEvaluation evaluation);
        Task<List<PlayerEvaluation>> GetUnsyncedAsync();
        Task DeleteAsync(string objectId);
        Task CreateEvaluationAsync(PlayerEvaluation evaluation);
        Task<List<string>> GetEvaluadosIds(string objectId);
    }

}
