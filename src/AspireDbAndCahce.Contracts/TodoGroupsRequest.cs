namespace AspireDbAndCahce.Contracts;

public class TodoGroupsRequest()
{
    public static TodoGroupsRequest Default => new TodoGroupsRequest
    {
        P = 1,
        Size = 10
    };

    private int page;
    private int size;

    public int P
    {
        get
        {
            return page > 0 ? page : 1;
        }
        set => page = value;
    }

    public int Size
    {
        get
        {
            return size > 0 ? size : 10;
        }
        set => size = value;
    }

    public string? Search { get; set; } = null;
}

