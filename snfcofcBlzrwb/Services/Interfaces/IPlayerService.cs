using snfcofcBlzrwb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAllAsync();
        Task<Player> GetByIdAsync(string objectId);
        Task SaveAsync(Player player);
        Task DeleteAsync(string objectId);
    }
}
