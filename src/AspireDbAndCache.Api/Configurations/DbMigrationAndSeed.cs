
using AspireDbAndCache.Api.Context;
using AspireDbAndCache.Api.Data;
using AspireDbAndCahce.Contracts.Enums;
using Bogus;

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
                var rnd = new Random();

                await Task.Delay(rnd.Next(100, 200), cancellationToken);

                var dbContext = serviceProviderScoped.GetRequiredService<ApplicationDbContext>();

                // Apply migrations
                await dbContext.Database.EnsureCreatedAsync(cancellationToken);

                // Seed data if necessary
                if (!dbContext.TodoGroups.Any())
                {
                    // TODO Bogus: Add your seeding logic here
                    var fakeTodoItem = new Faker<TodoItem>()
                        .RuleFor(i => i.Name, f => f.Lorem.Word())
                        .RuleFor(i => i.Description, f => f.Lorem.Sentence())

                        .RuleFor(i => i.Priority, f => f.PickRandom(TodoPriority.Low, TodoPriority.Medium, TodoPriority.Urgent, TodoPriority.High))
                        .RuleFor(i => i.Fixed, f => f.Random.Bool());

                    var fakeTodoGroup = new Faker<TodoGroup>()
                        .RuleFor(g => g.Name, f => f.Lorem.Word())
                        .RuleFor(g => g.Description, f => f.Lorem.Sentence())
                        .RuleFor(g => g.CreatedAt, f => f.Date.Past().ToUniversalTime())
                        .RuleFor(g => g.Items, f => fakeTodoItem.Generate(rnd.Next(0, 10)))
                        .Generate(10);

                    await dbContext.TodoGroups.AddRangeAsync(fakeTodoGroup, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
