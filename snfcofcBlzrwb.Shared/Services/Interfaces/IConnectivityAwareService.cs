using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Interfaces
{
    public interface IConnectivityAwareService
    {
        bool IsOnline { get; }
        void SetConnectivity(bool isOnline);
    }
}
