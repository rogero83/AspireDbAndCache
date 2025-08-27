namespace AspireDbAndCahce.Contracts
{
    public static class ExpenseTrackerEndPoints
    {
        public const string GetExpenses = "/api/expenses";
        public const string GetExpenseById = "/api/expenses/{id}";
        public const string CreateExpense = "/api/expenses";
        public const string UpdateExpense = "/api/expenses/{id}";
        public const string DeleteExpense = "/api/expenses/{id}";

        public const string GetCategories = "/api/categories";
        public const string GetCategoryById = "/api/categories/{id}";
        public const string CreateCategory = "/api/categories";
        public const string UpdateCategory = "/api/categories/{id}";
        public const string DeleteCategory = "/api/categories/{id}";
    }
}
