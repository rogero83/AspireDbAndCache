using AspireDbAndCache.Api.Common;
using AspireDbAndCache.Api.Context;
using AspireDbAndCache.Api.Interfaces;
using AspireDbAndCahce.Contracts;
using ZiggyCreatures.Caching.Fusion;

namespace AspireDbAndCache.Api.Endpoints
{
    public static class CategoriesEndpoint
    {
        public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet(ExpenseTrackerEndPoints.GetCategories, async (
                [AsParameters] CategoryListRequest request,
                ICategoryService categoryService,
                CancellationToken ct) =>
            {
                var result = await categoryService.GetCategoriesAsync(request, ct);
                return result.ToResult();

            }).Produces<CategoryListResponse>()
            .WithTags("Category")
            .WithOpenApi();

            app.MapPost(ExpenseTrackerEndPoints.CreateCategory, async (
                EditCategoryRequest request,
                ICategoryService categoryService,
                CancellationToken ct) =>
            {
                var result = await categoryService.CreateCategoryAsync(request, ct);
                return result.ToResult();

            }).Produces<CategoryEditedResponse>()
            .WithTags("Category")
            .WithOpenApi();

            app.MapPut(ExpenseTrackerEndPoints.UpdateCategory, async (
                int id,
                EditCategoryRequest request,
                ICategoryService categoryService,
                CancellationToken ct) =>
            {
                var result = await categoryService.UpdateCategoryAsync(id, request, ct);
                return result.ToResult();

            }).Produces<CategoryEditedResponse>()
            .WithTags("Category")
            .WithOpenApi();

            app.MapGet(ExpenseTrackerEndPoints.GetCategoryById, async (
                int id,
                IFusionCache cache,
                ApplicationDbContext db,
                ICategoryService categoryService,
                CancellationToken ct) =>
            {
                var result = await categoryService.GetCategoryById(id, ct);
                return result.ToResult();

            })
            .Produces<CategoryListModel>()
            .WithTags("Category")
            .WithOpenApi();

            return app;
        }
    }
}
