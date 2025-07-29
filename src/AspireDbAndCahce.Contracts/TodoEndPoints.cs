namespace AspireDbAndCahce.Contracts
{
    public static class TodoEndPoints
    {
        public const string GetTodoGroups = "/api/todogroups";
        public const string CreateTodoGroup = "/api/todogroup";
        public const string UpdateTodoGroup = "/api/todogroup";
        public const string DeleteTodoGroup = "/api/todogroup/{id}";
        public const string GetTodoGroupById = "/api/todogroup/{id}";

        public const string GetTodoItemsByGroupId = "/api/todogroup/{id}/items";
    }
}
