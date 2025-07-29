namespace AspireDbAndCahce.Contracts;

public record TodoGroupsResponse(IList<TodoGroupItemResponse> TodoGroups, int Page, int TotalPages, TodoGroupsRequest Request);

public record TodoGroupItemResponse(int Id,
    DateTime CreatedAt,
    string Name,
    string? Description,
    int CountTodo,
    int FixedTodo);