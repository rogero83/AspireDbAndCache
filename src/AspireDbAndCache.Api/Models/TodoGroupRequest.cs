namespace AspireDbAndCache.Api.Models;

public record TodoGroupRequest(string Name, string Description);

public record TodoGroupEditRequest(int Id, string Name, string? Description);