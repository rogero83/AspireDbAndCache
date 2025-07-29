using AspireDbAndCahce.Contracts.Enums;

namespace AspireDbAndCahce.Contracts;

public record TodoItemReponse(int Id, string Name, string? Description, TodoPriority Priority, bool Fixed);
