using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using snfcofcBlzrwb.Shared.Services.Implementations;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Services.Local;
using snfcofcBlzrwb.Shared.Services.Remote;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<IPlayerService, PlayerService>();
//builder.Services.AddSingleton<IPlayerService, PlayerLocalService>();
//builder.Services.AddSingleton<IPlayerService, PlayerRemoteService>();
await builder.Build().RunAsync();
builder.Services.AddSyncfusionBlazor();
