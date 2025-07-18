using AspireDbAndCache.Web.Context;
using AspireDbAndCache.Web.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Configurazione del database PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("mydb")));

builder.AddRedisDistributedCache("redis");

builder.Services.AddFusionCache()
    .WithRegisteredMemoryCache()
    .WithSerializer(new FusionCacheSystemTextJsonSerializer())
    .WithDistributedCache(new RedisCache(new RedisCacheOptions
    {
        ConnectionMultiplexerFactory = () => Task.FromResult(builder.Services.BuildServiceProvider().GetRequiredService<IConnectionMultiplexer>())
    }))
    .WithDefaultEntryOptions(new FusionCacheEntryOptions
    {
        // Cache duration inMemory
        Duration = TimeSpan.FromSeconds(10),
        // Cache duration in distributed cache
        DistributedCacheDuration = TimeSpan.FromSeconds(20),
    })
    .WithBackplane(new RedisBackplane(new RedisBackplaneOptions
    {
        ConnectionMultiplexerFactory = () => Task.FromResult(builder.Services.BuildServiceProvider()!.GetRequiredService<IConnectionMultiplexer>()),
    }));

builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        LocalCacheExpiration = TimeSpan.FromSeconds(5),
        Expiration = TimeSpan.FromSeconds(10),
        // Solo per test, disabilito la traccia locale
        Flags = HybridCacheEntryFlags.DisableLocalCache
    };
});

// Configurazione Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Migrazione automatica del database all'avvio
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Applica tutte le migrazioni pendenti
        dbContext.Database.Migrate();
        Console.WriteLine("Database migrato con successo!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Errore durante la migrazione: {ex.Message}");
    }
}

// Configure Swagger for API documentation
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Todos API V1");
    });
//}

// API Endpoints per Articoli
app.MapTodoGroupsEndpoints();

await app.RunAsync();