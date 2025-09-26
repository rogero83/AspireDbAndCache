using AspireDbAndCache.Api.Common;
using AspireDbAndCahce.Contracts;

namespace AspireDbAndCache.Api.Interfaces
{
    public interface IExpenseService
    {
        Task<Result<ExpenseEditedResponse>> CreateExpenseAsync(EditExpenseRequest request, CancellationToken ct);
        Task<Result<bool>> DeleteExpenseAsync(int id, CancellationToken ct);
        Task<Result<ExpensesListResponse>> GetAllExpensesAsync(int? page, CancellationToken ct);
        Task<Result<EditExpenseRequest>> GetExpenseByIdAsync(int id, CancellationToken ct);
        Task<Result<decimal>> GetExpensesAmount(CancellationToken ct);
        Task<Result<ExpenseEditedResponse>> UpdateExpenseAsync(int id, EditExpenseRequest request, CancellationToken ct);
    }
}
