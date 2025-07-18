using System.ComponentModel.DataAnnotations;

namespace AspireDbAndCache.Api.Data;

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