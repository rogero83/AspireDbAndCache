namespace AspireDbAndCahce.Contracts
{
    public class EditCategoryRequest()
    {
        public required string Name { get; set; }
        public string Color { get; set; } = "#3B82F6"; // Default blue
        public string Icon { get; set; } = "cart-fill"; // Default icon
    }
}
