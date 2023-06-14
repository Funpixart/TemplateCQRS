namespace TemplateCQRS.WebApp.Data.Pagination;

public class PagedResult<T>
{
    public PagedResult()
    {
        Results = new List<T>();
    }

    public PagedResult(IReadOnlyList<T> list)
    {
        Results = list;
    }

    public IReadOnlyList<T> Results { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int RowCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public bool IsFirst => CurrentPage == 1;
    public bool IsLast => CurrentPage == TotalPages;

    public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

    public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
}
