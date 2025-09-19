using AspireDbAndCache.Api.Common;
using AspireDbAndCahce.Contracts;

namespace AspireDbAndCache.Api.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<CategoryEditedResponse>> CreateCategoryAsync(EditCategoryRequest request, CancellationToken ct);
        Task<Result<CategoryListResponse>> GetCategoriesAsync(CategoryListRequest request, CancellationToken ct);
        Task<Result<CategoryListModel>> GetCategoryById(int id, CancellationToken ct);
        Task<Result<CategoryEditedResponse>> UpdateCategoryAsync(int id, EditCategoryRequest request, CancellationToken ct);
    }
}
