using Microsoft.Extensions.Logging;
using snfcofcBlzrwb.Shared.Data;
using snfcofcBlzrwb.Shared.Services;
using snfcofcBlzrwb.Shared.Services.Implementations;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Services.Remote;
using snfcofcBlzrwb.Shared.Services.Sync;
using SQLite;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor.Notifications;

namespace snfcofcBlzrwb
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZedXVTQmRcWExzWEJWYEg=");

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

            // 🔹 Registramos DatabaseService como singleton
            builder.Services.AddSingleton<DatabaseService>();

            // 🔹 Registramos SQLiteAsyncConnection de forma diferida, SIN bloquear con .Wait()
            builder.Services.AddSingleton<Task<SQLiteAsyncConnection>>(async provider =>
            {
                await DatabaseService.InitAsync();
                return DatabaseService.GetConnection();
            });

            builder.Services.AddScoped<AuthRemoteService>();
            builder.Services.AddScoped<IAuthService, AuthenticationService>();
            builder.Services.AddSingleton<ToastService>();
            builder.Services.AddScoped<IPlayerService, PlayerRemoteService>();
            builder.Services.AddScoped<IMatchService, MatchRemoteService>();
            builder.Services.AddScoped<IEvaluationService, EvaluationRemoteService>();
            builder.Services.AddScoped<ITeamService, TeamsRemoteService>();
            builder.Services.AddScoped<ConnectivityService>();
            builder.Services.AddScoped<HttpClient>();

            builder.Services.AddSingleton(new AppSettings
            {
                ApplicationId = "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB",
                RestApiKey = "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c",
                ParseBaseUrl = "https://parseapi.back4app.com/"
            });

            builder.Services.AddHttpClient();
            builder.Services.AddBlazorWebViewDeveloperTools();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
