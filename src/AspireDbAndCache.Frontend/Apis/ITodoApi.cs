using AspireDbAndCahce.Contracts;
using Refit;

namespace AspireDbAndCache.Frontend.Apis
{
    public interface ITodoApi
    {
        [Get(TodoEndPoints.GetTodoGroups)]
        Task<TodoGroupsResponse> GetTodoGroupsAsync(TodoGroupsRequest request, CancellationToken ct = default);

        [Get(TodoEndPoints.GetTodoGroupById)]
        Task<TodoGroupResponse> GetTodoGroupByIdAsync(int id, CancellationToken ct = default);

        [Get(TodoEndPoints.GetTodoItemsByGroupId)]
        Task<List<TodoItemReponse>> GetTodoItemsByGroupIdAsync(int id, CancellationToken ct = default);
    }
}
