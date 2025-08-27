using System.ComponentModel.DataAnnotations;

namespace AspireDbAndCache.Api.Data
{
    // Entità Category
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(7)]
        public string Color { get; set; } = "#3B82F6"; // Default blue

        [MaxLength(20)]
        public string Icon { get; set; } = "cart-fill";

        // Navigation property - Una categoria può avere molte spese
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
