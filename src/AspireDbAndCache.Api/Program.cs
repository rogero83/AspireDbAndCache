using AspireDbAndCache.Api.Configurations;
using AspireDbAndCache.Api.Context;
using AspireDbAndCache.Api.Endpoints;
using Microsoft.EntityFrameworkCore;
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

// Configurazione di Redis
builder.AddRedisDistributedCache("redis");

// Configurazione di Fusion Cache
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<FusionCacheSystemTextJsonSerializer>();
builder.Services.AddFusionCache()
    .WithRegisteredMemoryCache()
    .WithDefaultEntryOptions(new FusionCacheEntryOptions
    {
        // Cache duration inMemory
        Duration = TimeSpan.FromSeconds(10),
        // Cache duration in distributed cache
        DistributedCacheDuration = TimeSpan.FromSeconds(20),
    })
    .WithSerializer(sp => sp.GetRequiredService<FusionCacheSystemTextJsonSerializer>())
    .WithDistributedCache(sp =>
    {
        var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
        return new RedisCache(new RedisCacheOptions
        {
            ConnectionMultiplexerFactory = () => Task.FromResult(multiplexer)
        });
    })
    .WithBackplane(sp =>
    {
        var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
        return new RedisBackplane(new RedisBackplaneOptions
        {
            ConnectionMultiplexerFactory = () => Task.FromResult(multiplexer)
        });
    });

// Configurazione Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
//{
//    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
//});

builder.Services.AddHostedService<DbMigrationAndSeed>();

Mapping.Configure();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure Swagger for API documentation
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Todos API V1");
});

// API Endpoints
app
    .MapCategoriesEndpoints()
    .MapExpensesEndpoint();

await app.RunAsync();