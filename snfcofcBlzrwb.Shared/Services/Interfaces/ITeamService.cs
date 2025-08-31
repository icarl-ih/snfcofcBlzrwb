using snfcofcBlzrwb.Models;
using snfcofcBlzrwb.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Interfaces
{
    public interface ITeamService: IConnectivityAwareService
    {
        Task<List<TeamModel>> GetAllAsync();
        Task<TeamModel> GetByIdAsync(string objectId);
        Task SaveAsync (TeamModel model);
        Task DeleteAsync(string objectId);
        Task CreateTeamAsync(TeamModel team);
        Task<(string name, string url)> SubirFotoTeamAsync(byte[] data, string nombreArchivo);
    }
}
