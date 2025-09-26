namespace AspireDbAndCahce.Contracts
{
    public class CategoryListResponse
    {
        public IList<CategoryListModel> Categories { get; set; } = new List<CategoryListModel>();

        public int TotalPages { get; set; }
        public int Page { get; set; } = 1;
        public int ItemByPage { get; set; } = 10;
    }

    public record CategoryListModel()
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public int CountExpenses { get; set; }
        public decimal TotalExpenses { get; set; } = 0.0m;
        public decimal TotalIncome { get; set; } = 0.0m;

        public decimal Balance => TotalIncome - TotalExpenses;
    }

    public record CategoryEditedResponse(int Id);
}
