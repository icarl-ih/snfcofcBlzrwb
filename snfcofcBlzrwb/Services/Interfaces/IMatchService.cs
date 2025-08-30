using snfcofcBlzrwb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Services.Interfaces
{
    
    public interface IMatchService
    {
        Task<List<Match>> GetAllAsync();
        Task<Match> GetByIdAsync(string objectId);
        Task SaveAsync(Match match);
        Task DeleteAsync(string objectId);
    }
}
