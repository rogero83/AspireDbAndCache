using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspireDbAndCache.Api.Data
{
    // Entità Expense
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        public CashFlowType CashFlow { get; set; } = CashFlowType.Expense;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "L'importo deve essere maggiore di 0")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property - Ogni spesa appartiene a una categoria
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;
    }

    public enum CashFlowType
    {
        Expense = 0,
        Income = 1
    }
}
