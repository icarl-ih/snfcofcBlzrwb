using snfcofcBlzrwb.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb
{
    public class MauiUiDispatcher : IUiDispatcher
    {
        public Task InvokeOnMainThreadAsync(Func<Task> action) =>
            MainThread.InvokeOnMainThreadAsync(action);

        public void InvokeOnMainThread(Action action) =>
            MainThread.BeginInvokeOnMainThread(action);
    }
}
