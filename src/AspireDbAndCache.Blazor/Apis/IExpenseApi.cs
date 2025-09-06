using AspireDbAndCahce.Contracts;
using Refit;

namespace AspireDbAndCache.Blazor.Apis
{
    public interface IExpenseApi
    {
        [Get(ExpenseTrackerEndPoints.GetCategories)]
        Task<CategoryListResponse> GetCategoriesAsync(CategoryListRequest request, CancellationToken ct = default);

        [Get(ExpenseTrackerEndPoints.GetCategoryById)]
        Task<CategoryListModel> GetCategoryAsync(int id, CancellationToken ct = default);

        [Post(ExpenseTrackerEndPoints.CreateCategory)]
        Task<CategoryEditedResponse> CreateCategoryAsync(EditCategoryRequest request, CancellationToken ct = default);

        [Put(ExpenseTrackerEndPoints.UpdateCategory)]
        Task<CategoryEditedResponse> UpdateCategoryAsync(int id, EditCategoryRequest request, CancellationToken ct = default);
    }
}
