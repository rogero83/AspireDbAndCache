using AspireDbAndCache.Api.Common;
using AspireDbAndCache.Api.Interfaces;
using AspireDbAndCahce.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AspireDbAndCache.Api.Endpoints
{
    public static class ExpensesEndpoint
    {
        public static IEndpointRouteBuilder MapExpensesEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet(ExpenseTrackerEndPoints.GetExpenses, async (
                [FromQuery] int? page,
                IExpenseService expenseService,
                CancellationToken ct) =>
            {
                var result = await expenseService.GetAllExpensesAsync(page, ct);
                return result.ToResult();

            }).Produces<ExpensesListResponse>()
            .WithTags("Expense")
            .WithOpenApi();

            app.MapGet(ExpenseTrackerEndPoints.GetExpensesAmount, async (
                IExpenseService expenseService,
                CancellationToken ct) =>
            {
                var result = await expenseService.GetExpensesAmount(ct);
                return result.ToResult();

            })
            .WithTags("Expense")
            .WithOpenApi();

            app.MapGet(ExpenseTrackerEndPoints.GetExpenseById, async (
                [FromRoute] int id,
                IExpenseService expenseService,
                CancellationToken ct) =>
            {
                var result = await expenseService.GetExpenseByIdAsync(id, ct);
                return result.ToResult();

            }).Produces<EditExpenseRequest>()
            .WithTags("Expense")
            .WithOpenApi();

            app.MapPost(ExpenseTrackerEndPoints.CreateExpense, async (
                [FromBody] EditExpenseRequest request,
                IExpenseService expenseService,
                CancellationToken ct) =>
            {
                var result = await expenseService.CreateExpenseAsync(request, ct);
                return result.ToResult();

            }).Produces<ExpenseEditedResponse>()
            .WithTags("Expense")
            .WithOpenApi();

            app.MapPut(ExpenseTrackerEndPoints.UpdateExpense, async (
                [FromRoute] int id,
                [FromBody] EditExpenseRequest request,
                IExpenseService expenseService,
                CancellationToken ct) =>
            {
                var result = await expenseService.UpdateExpenseAsync(id, request, ct);
                return result.ToResult();

            }).Produces<ExpenseEditedResponse>()
            .WithTags("Expense")
            .WithOpenApi();

            app.MapDelete(ExpenseTrackerEndPoints.DeleteExpense, async (
                [FromRoute] int id,
                IExpenseService expenseService,
                CancellationToken ct) =>
            {
                var result = await expenseService.DeleteExpenseAsync(id, ct);
                return result.ToResult();
            }).WithTags("Expense")
            .WithOpenApi();

            return app;
        }
    }
}
