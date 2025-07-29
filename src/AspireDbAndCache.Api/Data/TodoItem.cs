using AspireDbAndCahce.Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace AspireDbAndCache.Api.Data;

public class TodoItem
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public TodoPriority Priority { get; set; }

    public bool Fixed { get; set; } = false;

    public int TodoGroupId { get; set; }

    // Navigation property
    public required virtual TodoGroup Group { get; set; }
}
