using AspireDbAndCache.Api.Context;
using AspireDbAndCahce.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspireDbAndCache.Api.Endpoints
{
    public static class TodosEndpoint
    {
        public static IEndpointRouteBuilder MapTodosEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet(TodoEndPoints.GetTodoItemsByGroupId, async (int id,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var result = await cache.GetOrSetAsync($"todoitems-{id}", async ct =>
                {
                    return await db.TodoItems
                        .Where(ti => ti.TodoGroupId == id)
                        .ProjectToType<TodoItemReponse>()
                        .ToListAsync(ct);
                },
                token: ct);

                return Results.Ok(result);
            }).Produces<List<TodoItemReponse>>()
            .WithTags("TodoItems")
            .WithOpenApi();

            return app;
        }
    }
}
