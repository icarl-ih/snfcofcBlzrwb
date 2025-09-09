using snfcofcBlzrwb.Shared.Services;
using snfcofcBlzrwb.Shared.Services.Interfaces;
namespace snfcofcBlzrwb.Web
{
    public class WebUiDispatcher : IUiDispatcher
    {
        public Task InvokeOnMainThreadAsync(Func<Task> action) => action();
        public void InvokeOnMainThread(Action action) => action();
    }
}
