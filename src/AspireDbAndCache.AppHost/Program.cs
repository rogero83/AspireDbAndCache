using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Enable docker publisher
builder.AddDockerComposeEnvironment("AspireDbAndCache");

var pgsql = builder.AddPostgres("pgsql")
    .WithDataVolume("todo-mydb")
    .WithLifetime(ContainerLifetime.Persistent);



var pgAdmin = pgsql.WithPgAdmin(x=>
{
    x.WithLifetime(ContainerLifetime.Persistent);
    x.WithHostPort(8080);
})
.WithLifetime(ContainerLifetime.Persistent);

var db = pgsql.AddDatabase("mydb");

var redis = builder.AddRedis("redis")
    .WithDataVolume("todo-redis")
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.AspireDbAndCache_Api>("aspiredbandcache-api")    
    .WithExternalHttpEndpoints()
    .WithReplicas(3)
    .WithReference(db).WaitFor(db)
    .WithReference(redis).WaitFor(redis);

builder.Build().Run();
