namespace AspireDbAndCache.Api.Endpoints
{
    public static class ExpensesEndpoint
    {
        public static IEndpointRouteBuilder MapExpensesEndpoint(this IEndpointRouteBuilder app)
        {
            //app.MapGet(ExpenseTrackerEndPoints.GetTodoItemsByGroupId, async (int id,
            //    IFusionCache cache,
            //    ApplicationDbContext db,
            //    CancellationToken ct) =>
            //{
            //    var result = await cache.GetOrSetAsync($"todoitems-{id}", async ct =>
            //    {
            //        return await db.TodoItems
            //            .Where(ti => ti.TodoGroupId == id)
            //            .ProjectToType<TodoItemReponse>()
            //            .ToListAsync(ct);
            //    },
            //    token: ct);

            //    return Results.Ok(result);
            //}).Produces<List<TodoItemReponse>>()
            //.WithTags("TodoItems")
            //.WithOpenApi();

            //app.MapGet(ExpenseTrackerEndPoints.GetTodoItemById, async (int id, ApplicationDbContext db, CancellationToken ct) =>
            //{
            //    var todoItem = await db.TodoItems.FindAsync(id, ct);
            //    return todoItem == null ? Results.NotFound() : Results.Ok(todoItem.Adapt<TodoItemEditRequest>());
            //})
            //.WithTags("TodoItems")
            //.WithOpenApi();

            //app.MapPut(ExpenseTrackerEndPoints.UpdateTodoItem, async ([FromBody] TodoItemEditRequest request,
            //    ApplicationDbContext db,
            //    IFusionCache cache,
            //    CancellationToken ct) =>
            //{
            //    var todoItem = await db.TodoItems.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            //    if (todoItem == null)
            //    {
            //        return Results.NotFound();
            //    }

            //    request.Adapt(todoItem);
            //    await db.SaveChangesAsync(ct);

            //    await cache.RemoveAsync($"todoitems-{todoItem.TodoGroupId}", token: ct);

            //    return Results.Ok(todoItem.Adapt<TodoItemReponse>());
            //})
            //.WithTags("TodoItems")
            //.WithOpenApi();

            return app;
        }
    }
}
