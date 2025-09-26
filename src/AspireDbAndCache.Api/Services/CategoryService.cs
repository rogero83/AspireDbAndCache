using AspireDbAndCache.Api.Common;
using AspireDbAndCache.Api.Context;
using AspireDbAndCache.Api.Data;
using AspireDbAndCache.Api.Interfaces;
using AspireDbAndCache.Api.Utility;
using AspireDbAndCahce.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AspireDbAndCache.Api.Services
{
    public class CategoryService(
        ApplicationDbContext db,
        ICacheService cacheService)
        : ICategoryService
    {
        public async Task<Result<CategoryEditedResponse>> CreateCategoryAsync(EditCategoryRequest request, CancellationToken ct)
        {
            var category = new Category
            {
                Name = request.Name,
                Color = request.Color,
                Icon = request.Icon
            };

            db.Categories.Add(category);
            await db.SaveChangesAsync(ct);
            if (category.Id == 0)
            {
                return ErrorResult.Create("Failed to create category");
            }

            await cacheService.RemoveByTagAsync(CacheKey.Tags.AllCategories, ct);

            return new CategoryEditedResponse(category.Id);
        }

        public async Task<Result<CategoryListResponse>> GetCategoriesAsync(CategoryListRequest request, CancellationToken ct)
        {
            var requestHasCode = HashCodeGenerator.GetObjectHashCode(request);

            var result = await cacheService.GetOrSetAsync(CacheKey.CategoryByHash(requestHasCode), async ct =>
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
            }, [CacheKey.Tags.AllCategories], ct);

            return result;
        }

        public async Task<Result<CategoryListModel>> GetCategoryById(int id, CancellationToken ct)
        {
            var category = await cacheService.GetOrSetAsync(CacheKey.CategoryById(id), async (ct) =>
            {
                var category = await db.Categories
                    .Where(tg => tg.Id == id)
                    // TODO modello per il get
                    .ProjectToType<CategoryListModel>()
                    .FirstOrDefaultAsync(ct);
                return category;
            }, ct: ct);

            if (category == null)
            {
                return ErrorResult.Create("Category not found");
            }

            return category;
        }

        public async Task<Result<CategoryEditedResponse>> UpdateCategoryAsync(int id, EditCategoryRequest request, CancellationToken ct)
        {
            var category = await db.Categories
                    .FirstOrDefaultAsync(tg => tg.Id == id, ct);

            if (category == null)
            {
                return ErrorResult.Create("Category not found");
            }

            category.Name = request.Name;
            category.Color = request.Color;
            category.Icon = request.Icon;

            db.Categories.Update(category);
            await db.SaveChangesAsync(ct);

            await cacheService.RemoveByTagAsync(CacheKey.Tags.AllCategories, ct);
            await cacheService.RemoveByTagAsync(CacheKey.Tags.AllExpenses, ct);
            await cacheService.RemoveAsync(CacheKey.CategoryById(id), ct);

            return new CategoryEditedResponse(category.Id);
        }
    }
}

