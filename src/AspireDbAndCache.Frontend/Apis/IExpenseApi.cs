using AspireDbAndCahce.Contracts;
using Refit;

namespace AspireDbAndCache.Frontend.Apis
{
    public interface IExpenseApi
    {
        [Get(ExpenseTrackerEndPoints.GetCategories)]
        Task<CategoryListResponse> GetCategoriesAsync(CategoryListRequest request, CancellationToken ct = default);

        [Get(ExpenseTrackerEndPoints.GetCategoryById)]
        Task<CategoryListModel> GetCategoryAsync(int id, CancellationToken ct = default);

        [Post(ExpenseTrackerEndPoints.CreateCategory)]
        Task<CategoryListModel> CreateCategoryAsync(EditCategoryRequest request, CancellationToken ct = default);

        [Put(ExpenseTrackerEndPoints.UpdateCategory)]
        Task<CategoryListModel> UpdateCategoryAsync(int id, EditCategoryRequest request, CancellationToken ct = default);
    }
}
