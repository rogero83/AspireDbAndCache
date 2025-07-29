using AspireDbAndCache.Api.Data;
using AspireDbAndCahce.Contracts;
using Mapster;

namespace AspireDbAndCache.Api.Configurations
{
    public static class Mapping
    {
        public static void Configure()
        {
            TypeAdapterConfig<TodoGroup, TodoGroupItemResponse>
                .NewConfig()
                .Map(dest => dest.CountTodo, src => src.Items.Count)
                .Map(dest => dest.FixedTodo, src => src.Items.Where(x => x.Fixed).Count());

            TypeAdapterConfig<TodoItem, TodoItemReponse>.NewConfig();
        }
    }
}
