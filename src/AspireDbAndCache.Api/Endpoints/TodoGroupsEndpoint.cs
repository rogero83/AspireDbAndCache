using AspireDbAndCache.Web.Context;
using AspireDbAndCache.Web.Data;
using AspireDbAndCache.Web.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspireDbAndCache.Web.Endpoints
{
    public static class TodoGroupsEndpoint
    {
        public static IEndpointRouteBuilder MapTodoGroupsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/todogroups", async (IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var result = await cache.GetOrSetAsync("all-todogroup", async ct =>
                {
                    return await db.TodoGroups
                        .ProjectToType<TodoGroupsResponse>()
                        .ToListAsync(ct);
                },
                token: ct);

                return Results.Ok(result);
            }).Produces<List<TodoGroupsResponse>>()
            .WithTags("TodoGroups")
            .WithOpenApi();

            app.MapPost("/api/todogroup", async ([FromBody] TodoGroupRequest request,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var todoGroup = new TodoGroup
                {
                    Name = request.Name,
                    Description = request.Description
                };
                db.TodoGroups.Add(todoGroup);
                await db.SaveChangesAsync(ct);

                await cache.RemoveAsync("all-todogroup", token: ct);

                return Results.Created($"/api/todogroup/{todoGroup.Id}", todoGroup);
            })
            .WithTags("TodoGroups")
            .WithOpenApi();

            app.MapPut("/api/todogroup", async ([FromBody] TodoGroupEditRequest request,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var todoGroup = await db.TodoGroups
                    .FirstOrDefaultAsync(tg => tg.Id == request.Id, ct);

                if (todoGroup == null)
                {
                    return Results.NotFound();
                }

                todoGroup.Name = request.Name;
                todoGroup.Description = request.Description;

                db.TodoGroups.Update(todoGroup);
                await db.SaveChangesAsync(ct);

                await cache.RemoveAsync("all-todogroup", token: ct);
                await cache.RemoveAsync($"todogroup-{request.Id}", token: ct);

                return Results.Accepted($"/api/todogroup/{todoGroup.Id}", todoGroup);
            })
            .WithTags("TodoGroups")
            .WithOpenApi();

            app.MapGet("/api/todogroup/{id:int}", async (int id,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var todoGroup = await cache.GetOrSetAsync($"todogroup-{id}", async (ct) =>
                {
                    var todoGroup = await db.TodoGroups
                        .FirstOrDefaultAsync(tg => tg.Id == id, ct);
                    return todoGroup;
                }, token: ct);

                if (todoGroup == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(todoGroup);
            })
            .WithTags("TodoGroups")
            .WithOpenApi();

            return app;
        }
    }
}
