using snfcofcBlzrwb.Shared.Services.Local;
using snfcofcBlzrwb.Shared.Services.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Text.Json;
using Xamarin.Essentials;
using snfcofcBlzrwb.Shared.Services.Sync;

namespace snfcofcBlzrwb.Shared.Services.Sync
{
    public class SyncService
    {
        private readonly PlayerLocalService _local;
        private readonly PlayerRemoteService _remote;
        private readonly ConnectivityService _connectivity;


        public SyncService(PlayerLocalService local, PlayerRemoteService remote, ConnectivityService connectivity)
        {
            _local = local;
            _remote = remote; 
            _connectivity = connectivity;

        }

        public async Task SyncAllAsync()
        {
            if (!await _connectivity.IsOnlineAsync())
            {
                // Aquí puedes lanzar un evento, log o alerta visual
                return;
            }

            if (!await _connectivity.IsBack4AppAvailableAsync())
            {
                // Otra alerta si Back4App no responde
                return;
            }

            await SyncPlayersAsync();
            Preferences.Set("LastSyncUtc", DateTime.UtcNow.ToString("o"));
        }


        public async Task SyncPlayersAsync()
        {
            //var unsynced = await _local.GetAllAsync();
            //foreach (var player in unsynced.Where(p => !p.IsSynced))
            //{
            //    var success = await _remote.UploadAsync(player);
            //    if (success)
            //    {
            //        player.IsSynced = true;
            //        await _local.SaveAsync(player);
            //    }
            //}

            //var remotePlayers = await _remote.DownloadAllAsync();
            //foreach (var rp in remotePlayers)
            //{
            //    var local = await _local.GetByIdAsync(rp.ObjectId);
            //    if (local == null || rp.UpdatedAt > local.UpdatedAt)
            //        await _local.SaveAsync(rp);
            //}
        }

        private async Task<bool> IsOnlineAsync()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync("https://parseapi.back4app.com/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public string GetLastSyncInfo()
        {
            return Preferences.Get("LastSyncUtc", "Sin sincronización previa");
        }
    }
}
