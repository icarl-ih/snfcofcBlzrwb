using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Interfaces
{
    public interface IExportSaver
    {
        Task SaveAsync(byte[] bytes, string fileName, string contentType);
    }
}
