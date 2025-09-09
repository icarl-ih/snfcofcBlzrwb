using snfcofcBlzrwb.Shared.Services;
using snfcofcBlzrwb.Shared.Services.Implementations;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Services.Local;
using snfcofcBlzrwb.Shared.Services.Remote;
using snfcofcBlzrwb.Shared.Services.Sync;
using snfcofcBlzrwb.Web;
using snfcofcBlzrwb.Web.Components;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();

// HttpClient con headers de Parse (mantén solo UNA forma de registrarlo)
builder.Services.AddHttpClient("ParseApi", client =>
{
    client.BaseAddress = new Uri("https://parseapi.back4app.com/");
    client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
    client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");
    // Opcional: sesiones revocables
    // client.DefaultRequestHeaders.Add("X-Parse-Revocable-Session", "1");
});

// Dispatcher para entorno Web
builder.Services.AddSingleton<IUiDispatcher, WebUiDispatcher>();

// Tus servicios
builder.Services.AddScoped<AuthRemoteService>();
builder.Services.AddScoped<IAuthService, AuthenticationService>();

// 👇 ToastService debe ser Scoped en Blazor Server
builder.Services.AddScoped<ToastService>();

builder.Services.AddScoped<IPlayerService, PlayerRemoteService>();
builder.Services.AddScoped<IMatchService, MatchRemoteService>();
builder.Services.AddScoped<IEvaluationService, EvaluationRemoteService>();
builder.Services.AddScoped<ITeamService, TeamsRemoteService>();
builder.Services.AddScoped<ConnectivityService>();

// ❌ Quita esta línea (duplicado):
// builder.Services.AddScoped<HttpClient>();

builder.Services.AddSingleton(new AppSettings
{
    ApplicationId = "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB",
    RestApiKey = "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c",
    ParseBaseUrl = "https://parseapi.back4app.com/"
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(snfcofcBlzrwb.Shared._Imports).Assembly,
        typeof(snfcofcBlzrwb.Web.Client._Imports).Assembly);

app.Run();
