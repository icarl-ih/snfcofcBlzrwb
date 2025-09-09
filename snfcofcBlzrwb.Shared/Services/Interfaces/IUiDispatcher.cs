using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services.Interfaces
{
    public interface IUiDispatcher
    {
        Task InvokeOnMainThreadAsync(Func<Task> action);
        void InvokeOnMainThread(Action action);
    }
}
