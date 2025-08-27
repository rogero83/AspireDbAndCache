namespace AspireDbAndCahce.Contracts
{
    public record CategoryListRequest(int Page, int ItemByPage = 20)
    {
        public static CategoryListRequest WithPage(int page) => new(page, 20);
    }
}
