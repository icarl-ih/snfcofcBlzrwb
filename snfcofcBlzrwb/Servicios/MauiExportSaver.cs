using snfcofcBlzrwb.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snfcofcBlzrwb.Servicios
{
    public class MauiExportSaver : IExportSaver
    {
        public async Task SaveAsync(byte[] bytes, string fileName, string contentType)
        {
            // Opción A: Cache + Share (rápida y funciona en Android/Windows)
            var path = Path.Combine(FileSystem.CacheDirectory, fileName);
            await File.WriteAllBytesAsync(path, bytes);

            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = fileName,
                File = new ShareFile(path, contentType)
            });

            // Opción B (Windows): Guardar directo a Descargas (descomenta si prefieres esto en Windows)
            /*
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                var downloads = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                Directory.CreateDirectory(downloads);
                var savePath = Path.Combine(downloads, fileName);
                await File.WriteAllBytesAsync(savePath, bytes);
            }
            */
        }
    }
}
