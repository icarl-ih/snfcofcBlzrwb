using snfcofcBlzrwb.Shared.Services.Interfaces;
using Syncfusion.Blazor.Notifications;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Shared.Services
{
    public class ToastService
    {
        private SfToast? _toastRef;
        private readonly IUiDispatcher _dispatcher;

        public ToastService(IUiDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Register(SfToast toastRef)
        {
            _toastRef = toastRef;
        }

        public Task ShowAsync(
            string content,
            string title = "Atención",
            string cssClass = "e-toast-info",
            int timeout = 4000)
        {
            if (_toastRef is null)
                return Task.CompletedTask;

            // Garantiza ejecución en el hilo de UI del WebView MAUI
            return _dispatcher.InvokeOnMainThreadAsync(async () =>
            {
                _toastRef.Content = content;
                _toastRef.Title = title;
                _toastRef.CssClass = cssClass;
                _toastRef.Timeout = timeout;

                await _toastRef.ShowAsync();
            });
        }

        public Task ShowSuccess(string message) =>
            ShowAsync(message, "✅ Éxito", "e-toast-success");

        public Task ShowError(string message) =>
            ShowAsync(message, "❌ Error", "e-toast-danger");

        public Task ShowWarning(string message) =>
            ShowAsync(message, "⚠️ Advertencia", "e-toast-warning");

        public Task ShowInfo(string message) =>
            ShowAsync(message, "ℹ️ Info", "e-toast-info");
    }
}
