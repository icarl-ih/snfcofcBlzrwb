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
    public class MatchService : IMatchService
    {
        private readonly IMatchService _local;
        private readonly IMatchService _remote;
        public bool IsOnline { get; private set; }

        public MatchService(IMatchService local, IMatchService remote)
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

        public Task<List<MatchModel>> GetAllAsync() =>
            IsOnline ? _remote.GetAllAsync() : _local.GetAllAsync();

        public Task<MatchModel?> GetByIdAsync(string objectId) =>
            IsOnline ? _remote.GetByIdAsync(objectId) : _local.GetByIdAsync(objectId);

        public Task SaveAsync(MatchModel match) =>
            IsOnline ? _remote.SaveAsync(match) : _local.SaveAsync(match);

        public Task DeleteAsync(string objectId) =>
            IsOnline ? _remote.DeleteAsync(objectId) : _local.DeleteAsync(objectId);

        public Task<List<MatchModel>> GetUnsyncedAsync() =>
            _local.GetUnsyncedAsync();
        public Task<(string name, string url)> SubirFotoEquipoAsync(byte[] data, string nombreArchivo) =>
             _remote.SubirFotoEquipoAsync(data, nombreArchivo);
        public Task CreateMatch(MatchModel match) => _remote.CreateMatch(match);

        public Task<List<MatchModel>> GetUnEvaluatedMatches(List<string> ids, Player player) => _remote.GetUnEvaluatedMatches(ids, player);
    }
}
