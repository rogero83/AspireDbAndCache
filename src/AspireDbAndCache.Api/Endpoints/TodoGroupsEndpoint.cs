using AspireDbAndCache.Api.Context;
using AspireDbAndCache.Api.Data;
using AspireDbAndCache.Api.Utility;
using AspireDbAndCahce.Contracts;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspireDbAndCache.Api.Endpoints
{
    public static class TodoGroupsEndpoint
    {
        public static IEndpointRouteBuilder MapTodoGroupsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet(TodoEndPoints.GetTodoGroups, async (
                [AsParameters] TodoGroupsRequest request,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var requestHasCode = HashCodeGenerator.GetObjectHashCode(request);

                var result = await cache.GetOrSetAsync($"all-todogroup-{requestHasCode}", async ct =>
                {
                    // apply filters
                    IQueryable<TodoGroup> query = db.TodoGroups;
                    if (!string.IsNullOrEmpty(request.Search))
                    {
                        query = query.Where(tg => tg.Name.Contains(request.Search));
                    }

                    var todoGroupItems = await query
                        .OrderBy(x => x.CreatedAt)
                        .Skip((request.P - 1) * request.Size)
                        .Take(request.Size)
                        .ProjectToType<TodoGroupItemResponse>()
                        .ToListAsync(ct);

                    var totalCount = await query.CountAsync(ct);

                    return new TodoGroupsResponse(
                        todoGroupItems,
                        request.P,
                        (int)Math.Ceiling((double)totalCount / request.Size),
                        request);
                },
                tags: ["all-todogroup"],
                token: ct);

                return Results.Ok(result);
            }).Produces<TodoGroupsResponse>()
            .WithTags("TodoGroups")
            .WithOpenApi();

            app.MapPost(TodoEndPoints.CreateTodoGroup, async ([FromBody] TodoGroupCreateRequest request,
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

                await cache.RemoveByTagAsync("all-todogroup", token: ct);

                return Results.Created($"/api/todogroup/{todoGroup.Id}", todoGroup);
            })
            .WithTags("TodoGroups")
            .WithOpenApi();

            app.MapPut(TodoEndPoints.UpdateTodoGroup, async ([FromBody] TodoGroupEditRequest request,
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

                await cache.RemoveByTagAsync("all-todogroup", token: ct);
                await cache.RemoveAsync($"todogroup-{request.Id}", token: ct);

                return Results.Accepted($"/api/todogroup/{todoGroup.Id}", todoGroup);
            })
            .WithTags("TodoGroups")
            .WithOpenApi();

            app.MapGet(TodoEndPoints.GetTodoGroupById, async (int id,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var todoGroup = await cache.GetOrSetAsync($"todogroup-{id}", async (ct) =>
                {
                    var todoGroup = await db.TodoGroups
                        .Where(tg => tg.Id == id)
                        .ProjectToType<TodoGroupResponse>()
                        .FirstOrDefaultAsync(ct);
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
