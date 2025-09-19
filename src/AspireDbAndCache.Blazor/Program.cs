using AspireDbAndCache.Blazor.Apis;
using AspireDbAndCache.Blazor.Components;
using Refit;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// refit fix for enum 
var serializer = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();
serializer.Converters.Remove(serializer.Converters.Single(x => x.GetType().Equals(typeof(JsonStringEnumConverter))));
var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(serializer)
};

builder.Services
    .AddRefitClient<IExpenseApi>(refitSettings)
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https+http://aspiredbandcache-api");
    });

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
