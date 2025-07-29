using AspireDbAndCache.Frontend.Apis;
using AspireDbAndCahce.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspireDbAndCache.Frontend.Pages.Todo
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITodoApi _todoApi;

        public IndexModel(ILogger<IndexModel> logger, ITodoApi todoApi)
        {
            _todoApi = todoApi;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }

        public List<TodoItemReponse> TodoItems { get; private set; }
        public TodoGroupResponse TodoGroup { get; private set; }

        public void OnGet()
        {
            var todoItemsTask = _todoApi.GetTodoItemsByGroupIdAsync(GroupId);
            var todoGroupTask = _todoApi.GetTodoGroupByIdAsync(GroupId);

            Task.WaitAll(todoItemsTask, todoGroupTask);

            TodoItems = todoItemsTask.Result;
            TodoGroup = todoGroupTask.Result;
        }
    }
}
