using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspireDbAndCache.Web.Data
{
    public class TodoGroup
    {
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; }

        // Navigation property
        public virtual ICollection<TodoItem> Items { get; set; } = [];
    }
}
