using AspireDbAndCache.Frontend.Apis;
using AspireDbAndCahce.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspireDbAndCache.Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ITodoApi _todoApi;

    public TodoGroupsResponse TodoGroupsResponse { get; set; }

    [BindProperty(SupportsGet = true)]
    public TodoGroupsRequest? TodoGroupsRequest { get; set; }

    public IndexModel(ILogger<IndexModel> logger,
        ITodoApi todoApi)
    {
        _logger = logger;
        _todoApi = todoApi;
    }

    public async Task OnGet()
    {
        TodoGroupsResponse = await _todoApi.GetTodoGroupsAsync(TodoGroupsRequest ?? TodoGroupsRequest.Default);
    }
}
