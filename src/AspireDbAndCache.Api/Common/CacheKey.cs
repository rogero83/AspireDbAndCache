namespace AspireDbAndCache.Api.Common
{
    public static class CacheKey
    {
        public static class Tags
        {
            public static string AllCategories => "categories_all";
            public static string AllExpenses => "expenses_all";
        }

        public static string CategoryByHash(int hash) => $"category_{hash}";
        public static string CategoryById(int id) => $"category_id_{id}";

        public static string ExpensesByPage(int page) => $"expenses_page_{page}";
        public static string ExpenseById(int id) => $"expense_id_{id}";
        public static string ExpensesAmount => $"expenses_amount";
    }
}
