using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using snfcofcBlzrwb.Shared.Services.Implementations;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Services.Remote;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JEaF5cXmRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZedXVWRWdYVEVyV0NWYEk=");
// 🔐 Servicios personalizados
builder.Services.AddScoped<AuthRemoteService>();
builder.Services.AddScoped<IAuthService, AuthenticationService>();
builder.Services.AddScoped<IPlayerService, PlayerRemoteService>();
builder.Services.AddScoped<IMatchService, MatchRemoteService>();
builder.Services.AddScoped<IEvaluationService, EvaluationRemoteService>();
builder.Services.AddScoped<ITeamService, TeamsRemoteService>();

// 🎨 Syncfusion Blazor
builder.Services.AddSyncfusionBlazor(); // ✅ Debe ir antes de Build()

// 🚀 Ejecutar la app
await builder.Build().RunAsync();