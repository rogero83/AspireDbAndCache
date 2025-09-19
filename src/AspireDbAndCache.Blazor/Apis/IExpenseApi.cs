using AspireDbAndCahce.Contracts;
using Refit;

namespace AspireDbAndCache.Blazor.Apis
{
    public interface IExpenseApi
    {
        #region Category
        [Get(ExpenseTrackerEndPoints.GetCategories)]
        Task<CategoryListResponse> GetCategoriesAsync(CategoryListRequest request, CancellationToken ct = default);

        [Get(ExpenseTrackerEndPoints.GetCategoryById)]
        Task<CategoryListModel> GetCategoryAsync(int id, CancellationToken ct = default);

        [Post(ExpenseTrackerEndPoints.CreateCategory)]
        Task<CategoryEditedResponse> CreateCategoryAsync(EditCategoryRequest request, CancellationToken ct = default);

        [Put(ExpenseTrackerEndPoints.UpdateCategory)]
        Task<CategoryEditedResponse> UpdateCategoryAsync(int id, EditCategoryRequest request, CancellationToken ct = default);
        #endregion Category

        #region Expense
        [Get(ExpenseTrackerEndPoints.GetExpenses)]
        Task<ExpensesListResponse> GetExpensesAsync(int? page = null, CancellationToken ct = default);

        [Get(ExpenseTrackerEndPoints.GetExpensesAmount)]
        Task<decimal> GetExpensesAmountAsync(CancellationToken ct = default);

        [Get(ExpenseTrackerEndPoints.GetExpenseById)]
        Task<EditExpenseRequest> GetExpenseAsync(int id, CancellationToken ct = default);

        [Post(ExpenseTrackerEndPoints.CreateExpense)]
        Task<ExpenseEditedResponse> CreateExpenseAsync(EditExpenseRequest request, CancellationToken ct = default);

        [Put(ExpenseTrackerEndPoints.UpdateExpense)]
        Task<ExpenseEditedResponse> UpdateExpenseAsync(int id, EditExpenseRequest request, CancellationToken ct = default);
        #endregion Expense
    }
}
