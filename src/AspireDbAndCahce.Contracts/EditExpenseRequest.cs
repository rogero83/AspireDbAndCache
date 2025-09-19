using AspireDbAndCahce.Contracts.Enums;

namespace AspireDbAndCahce.Contracts
{
    public class EditExpenseRequest
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public decimal Amount { get; set; }
        public CashFlowType CashFlow { get; set; }
        public int CategoryId { get; set; }
    }

    public record ExpenseEditedResponse(int Id);
}
