using AspireDbAndCache.Frontend.Apis;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspireDbAndCache.Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IExpenseApi _todoApi;

    //public TodoGroupsResponse TodoGroupsResponse { get; set; }

    //[BindProperty(SupportsGet = true)]
    //public TodoGroupsRequest? TodoGroupsRequest { get; set; }

    public IndexModel(ILogger<IndexModel> logger,
        IExpenseApi todoApi)
    {
        _logger = logger;
        _todoApi = todoApi;
    }

    public async Task OnGet(CancellationToken ct)
    {
        //TodoGroupsResponse = await _todoApi.GetTodoGroupsAsync(TodoGroupsRequest ?? TodoGroupsRequest.Default, ct);
    }
}
