using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using snfcofcBlzrwb.Shared.Data;
using snfcofcBlzrwb.Shared.Services;
using snfcofcBlzrwb.Shared.Services.Implementations;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Services.Local;
using snfcofcBlzrwb.Shared.Services.Remote;
using snfcofcBlzrwb.Shared.Services.Sync;
using SQLite;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using System.Net.Http;


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
            builder.Services.AddScoped<SfDialogService>();
            //builder.Services.AddSyncfusionBlazor();
            //builder.Services.AddSingleton<DialogService>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<SQLiteAsyncConnection>(provider => {
                DatabaseService.InitAsync().Wait();
                return DatabaseService.GetConnection();
            });
            builder.Services.AddScoped<snfcofcBlzrwb.Shared.Services.Interfaces.IAuthService, AuthRemoteService>(); 
            builder.Services.AddScoped<IPlayerService, PlayerRemoteService>();
            builder.Services.AddScoped<IMatchService, MatchRemoteService>();
            builder.Services.AddScoped<IEvaluationService, EvaluationRemoteService>();
            builder.Services.AddScoped<ITeamService, TeamsRemoteService>();
            builder.Services.AddScoped<RemoteService>();
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
            builder.Services.AddSingleton(new SQLiteAsyncConnection("ihsolutionsdb.db3"));
            builder.Services.AddHttpClient(); // Esto registra HttpClient para inyección
            builder.Services.AddSingleton<ConnectivityService>();
            builder.Services.AddHttpClient(); // Asegura que HttpClient esté disponible

            builder.Services.AddSingleton<ConnectivityService>();            
            builder.Services.AddBlazorWebViewDeveloperTools();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
