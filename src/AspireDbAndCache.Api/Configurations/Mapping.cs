using AspireDbAndCache.Api.Data;
using AspireDbAndCahce.Contracts;
using Mapster;

namespace AspireDbAndCache.Api.Configurations
{
    public static class Mapping
    {
        public static void Configure()
        {
            TypeAdapterConfig<Category, CategoryListModel>
                .NewConfig()
                .Map(dest => dest.CountExpenses, src => src.Expenses.Count)
                .Map(dest => dest.TotalExpenses, src => src.Expenses.Where(x => x.CashFlow == CashFlowType.Expense).Sum(x => x.Amount))
                .Map(dest => dest.TotalIncome, src => src.Expenses.Where(x => x.CashFlow == CashFlowType.Income).Sum(x => x.Amount));
        }
    }
}
