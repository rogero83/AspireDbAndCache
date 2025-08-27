using AspireDbAndCache.Api.Context;

namespace AspireDbAndCache.Api.Configurations
{
    public class DbMigrationAndSeed(IServiceProvider serviceProvider)
        : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var serviceProviderScoped = scope.ServiceProvider;

            var env = serviceProviderScoped.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment())
            {
                var dbContext = serviceProviderScoped.GetRequiredService<ApplicationDbContext>();

                // Apply migrations
                var result = await dbContext.Database.EnsureCreatedAsync(cancellationToken);

                // Seed data if necessary                
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
