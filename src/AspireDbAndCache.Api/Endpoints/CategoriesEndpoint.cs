using AspireDbAndCache.Api.Context;
using AspireDbAndCache.Api.Data;
using AspireDbAndCache.Api.Utility;
using AspireDbAndCahce.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspireDbAndCache.Api.Endpoints
{
    public static class CategoriesEndpoint
    {
        public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet(ExpenseTrackerEndPoints.GetCategories, async (
                [AsParameters] CategoryListRequest request,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var requestHasCode = HashCodeGenerator.GetObjectHashCode(request);

                var result = await cache.GetOrSetAsync($"get-categories-{requestHasCode}", async ct =>
                {
                    // apply filters
                    IQueryable<Category> query = db.Categories;

                    var category = await query.OrderBy(x => x.Name)
                        .Skip((request.Page - 1) * request.ItemByPage)
                        .Take(request.ItemByPage)
                        .ProjectToType<CategoryListModel>()
                        .ToListAsync(ct);

                    var totalCount = await query.CountAsync(ct);

                    return new CategoryListResponse
                    {
                        Categories = category,
                        TotalPages = (int)Math.Floor((decimal)totalCount / request.ItemByPage),
                        Page = request.Page,
                        ItemByPage = request.ItemByPage
                    };
                },
                tags: ["get-categories"],
                token: ct);

                return Results.Ok(result);
            }).Produces<CategoryListResponse>()
            .WithTags("Category")
            .WithOpenApi();

            app.MapPost(ExpenseTrackerEndPoints.CreateCategory, async (
                EditCategoryRequest request,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var category = new Category
                {
                    Name = request.Name,
                    Color = request.Color,
                    Icon = request.Icon
                };

                db.Categories.Add(category);
                await db.SaveChangesAsync(ct);

                await cache.RemoveByTagAsync("get-categories", token: ct);

                return Results.Ok(new CategoryEditedResponse(category.Id));
            }).Produces<CategoryEditedResponse>()
            .WithTags("Category")
            .WithOpenApi();

            app.MapPut(ExpenseTrackerEndPoints.UpdateCategory, async (
                int id,
                EditCategoryRequest request,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var category = await db.Categories
                    .FirstOrDefaultAsync(tg => tg.Id == id, ct);

                if (category == null)
                {
                    return Results.NotFound();
                }

                category.Name = request.Name;
                category.Color = request.Color;
                category.Icon = request.Icon;

                db.Categories.Update(category);
                await db.SaveChangesAsync(ct);

                await cache.RemoveByTagAsync("get-categories", token: ct);
                await cache.RemoveAsync($"category-{id}", token: ct);

                return Results.Ok(new CategoryEditedResponse(category.Id));
            }).Produces<CategoryEditedResponse>()
            .WithTags("Category")
            .WithOpenApi();

            app.MapGet(ExpenseTrackerEndPoints.GetCategoryById, async (
                int id,
                IFusionCache cache,
                ApplicationDbContext db,
                CancellationToken ct) =>
            {
                var category = await cache.GetOrSetAsync($"category-{id}", async (ct) =>
                {
                    var category = await db.Categories
                        .Where(tg => tg.Id == id)
                        // TODO modello per il get
                        .ProjectToType<CategoryListModel>()
                        .FirstOrDefaultAsync(ct);
                    return category;
                }, token: ct);

                if (category == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(category);
            })
            .Produces<CategoryListModel>()
            .WithTags("Category")
            .WithOpenApi();

            return app;
        }
    }
}
