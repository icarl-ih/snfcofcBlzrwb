using Microsoft.Extensions.Logging;
using snfcofcBlzrwb.Shared.Services;
using snfcofcBlzrwb.Shared.Data;
using snfcofcBlzrwb.Shared.Services.Sync;
using SQLite;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using snfcofcBlzrwb.Shared.Services.Remote;
using snfcofcBlzrwb.Shared.Services.Local;
using snfcofcBlzrwb.Shared.Services.Implementations;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using Syncfusion.Blazor;

namespace snfcofcBlzrwb
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<SQLiteAsyncConnection>(provider => {
                DatabaseService.InitAsync().Wait();
                return DatabaseService.GetConnection();
            });
            builder.Services.AddScoped<IPlayerService, PlayerRemoteService>();
            builder.Services.AddScoped<IMatchService, MatchRemoteService>();
            builder.Services.AddScoped<IEvaluationService, EvaluationRemoteService>();
            builder.Services.AddScoped<ITeamService, TeamsRemoteService>();
            //builder.Services.AddScoped<IPlayerService, PlayerLocalService>();
            // Servicios locales
            //builder.Services.AddSingleton<IPlayerService, PlayerLocalService>();
            //builder.Services.AddSingleton<IMatchService, MatchLocalService>();
            //builder.Services.AddSingleton<IEvaluationService, EvaluationLocalService>();
            //builder.Services.AddScoped<IPlayerService, PlayerService>();
            //builder.Services.AddScoped<IMatchService, MatchService>();
            //builder.Services.AddScoped<IEvaluationService, EvaluationService>();
            builder.Services.AddScoped<ConnectivityService>();
            builder.Services.AddScoped<HttpClient>();
            //builder.Services.AddSyncfusionBlazor();
            builder.Services.AddSingleton(new AppSettings
            {
                ApplicationId = "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB",
                RestApiKey = "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c",
                ParseBaseUrl = "https://parseapi.back4app.com/"
            });
            builder.Services.AddSingleton<ConnectivityService>();
            builder.Services.AddSingleton(new SQLiteAsyncConnection("players.db"));
            builder.Services.AddHttpClient(); // Esto registra HttpClient para inyección

            //        builder.Services.AddHttpClient<IPlayerService, PlayerRemoteService>()
            //.ConfigureHttpClient((sp, client) =>
            //{
            //    var settings = sp.GetRequiredService<AppSettings>();
            //    client.BaseAddress = new Uri(settings.ParseBaseUrl);
            //    client.DefaultRequestHeaders.Add("X-Parse-Application-Id", settings.ApplicationId);
            //    client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", settings.RestApiKey);
            //    client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            //});
            builder.Services.AddSingleton<ConnectivityService>();
            builder.Services.AddHttpClient(); // Asegura que HttpClient esté disponible

            builder.Services.AddSingleton<ConnectivityService>();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
