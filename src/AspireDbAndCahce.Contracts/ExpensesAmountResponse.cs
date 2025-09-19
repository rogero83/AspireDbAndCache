namespace AspireDbAndCahce.Contracts
{
    public class ExpensesAmountResponse
    {
        public decimal TotalAmount { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
