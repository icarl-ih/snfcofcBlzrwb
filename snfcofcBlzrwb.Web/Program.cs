using snfcofcBlzrwb.Shared.Services;
using snfcofcBlzrwb.Shared.Services.Implementations;
using snfcofcBlzrwb.Shared.Services.Interfaces;
using snfcofcBlzrwb.Shared.Services.Local;
using snfcofcBlzrwb.Shared.Services.Remote;
using snfcofcBlzrwb.Shared.Services.Sync;
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


//builder.Services.AddSingleton(new AppSettings
//{
//    ApplicationId = "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB",
//    RestApiKey = "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c",
//    ParseBaseUrl = "https://parseapi.back4app.com/"
//});
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient
    {
        BaseAddress = new Uri("https://parseapi.back4app.com/")
    };

    client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "6oKsUkJEbAocUPj5GiVdHlgTJlNMOLuyXqAda0yB");
    client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "OGtKUrtBgknWdLCjN9BVkzOuX4Q31MGgTw4ZZ96c");

    return client;
});
//builder.Services.AddHttpClient<PlayerRemoteService>((sp, client) =>
//{
//    var settings = sp.GetRequiredService<AppSettings>();
//    client.BaseAddress = new Uri(settings.ParseBaseUrl);
//    client.DefaultRequestHeaders.Add("X-Parse-Application-Id", settings.ApplicationId);
//    client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", settings.RestApiKey);
//    client.DefaultRequestHeaders.Add("Content-Type", "application/json");
//});
builder.Services.AddScoped<IPlayerService, PlayerRemoteService>();
builder.Services.AddScoped<IMatchService, MatchRemoteService>();
builder.Services.AddScoped<IEvaluationService, EvaluationRemoteService>();
builder.Services.AddScoped<ITeamService, TeamsRemoteService>();
////builder.Services.AddScoped<IPlayerService, PlayerLocalService>();

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
