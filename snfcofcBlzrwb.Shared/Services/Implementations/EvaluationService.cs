using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Implementations
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IEvaluationService _local;
        private readonly IEvaluationService _remote;
        public bool IsOnline { get; private set; }

        public EvaluationService(IEvaluationService local, IEvaluationService remote)
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

        public Task<List<PlayerEvaluation>> GetAllAsync() =>
            IsOnline ? _remote.GetAllAsync() : _local.GetAllAsync();

        public Task<PlayerEvaluation?> GetByIdAsync(string objectId) =>
            IsOnline ? _remote.GetByIdAsync(objectId) : _local.GetByIdAsync(objectId);

        public Task SaveAsync(PlayerEvaluation evaluation) =>
            IsOnline ? _remote.SaveAsync(evaluation) : _local.SaveAsync(evaluation);

        public Task DeleteAsync(string objectId) =>
            IsOnline ? _remote.DeleteAsync(objectId) : _local.DeleteAsync(objectId);

        public Task<List<PlayerEvaluation>> GetUnsyncedAsync() =>
            _local.GetUnsyncedAsync();
        public async Task CreateEvaluationAsync(PlayerEvaluation evaluation) => _remote.CreateEvaluationAsync(evaluation);
    }


}
