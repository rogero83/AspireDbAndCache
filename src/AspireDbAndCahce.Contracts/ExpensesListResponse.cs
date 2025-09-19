namespace AspireDbAndCahce.Contracts
{
    public class ExpensesListResponse
    {
        public required List<ExpenseResponse> Expenses { get; set; }
        public required int TotalCount { get; set; }
        public int PageSize { get; set; }
        public required int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public class ExpenseResponse
    {
        public required int Id { get; set; }
        public required string Description { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime Date { get; set; }
        public string? Note { get; set; }
        public required CategoryListModel Category { get; set; }
    }
}
