using Syncfusion.Blazor.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace snfcofcBlzrwb.Shared.Services
{
    public class ToastService
        {
            private SfToast? _toastRef;

            public void Register(SfToast toastRef)
            {
                _toastRef = toastRef;
            }

            public async Task ShowAsync(string content, string title = "Notificación", string cssClass = "e-toast-info", int timeout = 4000)
            {
            if (_toastRef != null)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    _toastRef.Content = content;
                    _toastRef.Title = title;
                    _toastRef.CssClass = cssClass;
                    _toastRef.Timeout = timeout;
                    await _toastRef.ShowAsync();
                });
            }
            }

            public async Task ShowSuccess(string message) =>
                await ShowAsync(message, "✅ Éxito", "e-toast-success");

            public async Task ShowError(string message) =>
                await ShowAsync(message, "❌ Error", "e-toast-danger");

            public async Task ShowWarning(string message) =>
                await ShowAsync(message, "⚠️ Advertencia", "e-toast-warning");

            public async Task ShowInfo(string message) =>
                await ShowAsync(message, "ℹ️ Info", "e-toast-info");
        }
    }
