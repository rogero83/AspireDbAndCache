using AspireDbAndCache.Api.Common;
using AspireDbAndCache.Api.Context;
using AspireDbAndCache.Api.Interfaces;
using AspireDbAndCahce.Contracts;
using AspireDbAndCahce.Contracts.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AspireDbAndCache.Api.Services
{
    public class ExpenseService(
        ApplicationDbContext db,
        ICacheService cacheService)
        : IExpenseService
    {
        private const int ITEMBYPAGE = 10;

        public async Task<Result<ExpenseEditedResponse>> CreateExpenseAsync(EditExpenseRequest request, CancellationToken ct)
        {
            // TODO add validator for request
            var expense = request.Adapt<Data.Expense>();
            expense.CreatedAt = DateTime.UtcNow;
            expense.UpdatedAt = DateTime.UtcNow;

            await db.Expenses.AddAsync(expense, ct);
            var saverd = await db.SaveChangesAsync(ct);
            if (saverd == 0)
            {
                return ErrorResult.Create("Failed to create expense.");
            }

            await cacheService.RemoveByTagAsync(CacheKey.Tags.AllExpenses, ct);

            return new ExpenseEditedResponse(expense.Id);
        }

        public async Task<Result<bool>> DeleteExpenseAsync(int id, CancellationToken ct)
        {
            var result = await db.Expenses.Where(x => x.Id == id).ExecuteDeleteAsync(ct);

            if (result == 0)
                return ErrorResult.Create("Expense not found.");

            await cacheService.RemoveByTagAsync(CacheKey.ExpenseById(id), ct);
            await cacheService.RemoveByTagAsync(CacheKey.Tags.AllExpenses, ct);

            return true;
        }

        public async Task<Result<ExpensesListResponse>> GetAllExpensesAsync(int? page, CancellationToken ct)
        {
            page = page.HasValue ? page.Value - 1 : 0;
            var result = await cacheService.GetOrSetAsync(CacheKey.ExpensesByPage(page.Value), async ct =>
            {
                var totalCount = await db.Expenses.CountAsync(ct);

                var expenses = await db.Expenses
                    .OrderByDescending(x => x.Date)
                    .Skip(ITEMBYPAGE * page.Value).Take(ITEMBYPAGE)
                    .ProjectToType<ExpenseResponse>()
                    .ToListAsync(ct);

                var result = new ExpensesListResponse
                {
                    TotalCount = totalCount,
                    CurrentPage = page.Value + 1,
                    Expenses = expenses,
                    PageSize = ITEMBYPAGE
                };

                return result;
            },
            tags: [CacheKey.Tags.AllExpenses],
            ct: ct);

            if (result.Expenses is null || result.Expenses.Count == 0)
            {
                return Result<ExpensesListResponse>.Problem("No expenses found.");
            }

            return result;
        }

        public async Task<Result<EditExpenseRequest>> GetExpenseByIdAsync(int id, CancellationToken ct)
        {
            var result = await cacheService.GetOrSetAsync(CacheKey.ExpenseById(id), async ct =>
            {
                var totalCount = await db.Expenses.CountAsync(ct);

                var expenses = await db.Expenses
                    .Where(x => x.Id == id)
                    .ProjectToType<EditExpenseRequest>()
                    .FirstOrDefaultAsync(ct);

                return expenses;
            },
            ct: ct);

            if (result is null)
            {
                return ErrorResult.Create("Not found");
            }

            return result;
        }

        public async Task<Result<decimal>> GetExpensesAmount(CancellationToken ct)
        {
            var result = await cacheService.GetOrSetAsync(CacheKey.ExpensesAmount, async ct =>
            {
                return await db.Expenses.SumAsync(x =>
                    (x.CashFlow == CashFlowType.Expense ? (-1) : 1)
                    * x.Amount, ct);
            },
            tags: [CacheKey.Tags.AllExpenses],
            ct: ct);

            return result;
        }

        public async Task<Result<ExpenseEditedResponse>> UpdateExpenseAsync(int id, EditExpenseRequest request, CancellationToken ct)
        {
            var expense = await db.Expenses.FirstOrDefaultAsync(x => x.Id == id, ct);

            if (expense is null)
            {
                return ErrorResult.Create("Expense not found.");
            }

            expense.UpdatedAt = DateTime.UtcNow;
            expense.Description = request.Description;
            expense.Date = request.Date;
            expense.CategoryId = request.CategoryId;
            expense.Notes = request.Notes;
            expense.Amount = request.Amount;
            expense.CashFlow = request.CashFlow;

            db.Expenses.Update(expense);
            await db.SaveChangesAsync(ct);

            await cacheService.RemoveByTagAsync(CacheKey.Tags.AllExpenses, ct);
            await cacheService.RemoveAsync(CacheKey.ExpenseById(id), ct);

            return new ExpenseEditedResponse(expense.Id);
        }
    }
}
