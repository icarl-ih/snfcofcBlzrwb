using snfcofcBlzrwb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Services.Interfaces
{
    public interface IEvaluationService
    {
        Task<List<PlayerEvaluation>> GetAllAsync();
        Task<PlayerEvaluation> GetByIdAsync(string objectId);
        Task SaveAsync(PlayerEvaluation evaluation);
        Task DeleteAsync(string objectId);
    }
    
}
