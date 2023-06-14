namespace TemplateCQRS.WebApp.Data.Pagination;

public static class PagedResultExtension
{
    public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : class
    {
        var result = new PagedResult<T>
        {
            CurrentPage = page,
            PageSize = pageSize,
            RowCount = query.Count()
        };

        var pageCount = (double)result.RowCount / pageSize;
        result.TotalPages = (int)Math.Ceiling(pageCount);

        var skip = (page - 1) * pageSize;
        result.Results = query.Skip(skip).Take(pageSize).ToList();

        return result;
    }

    public static PagedResult<T> GetPaged<T>(this List<T> query, int page, int pageSize) where T : class
    {
        var result = new PagedResult<T>
        {
            CurrentPage = page,
            PageSize = pageSize,
            RowCount = query.Count
        };

        var pageCount = (double)result.RowCount / pageSize;
        result.TotalPages = (int)Math.Ceiling(pageCount);

        var skip = (page - 1) * pageSize;
        result.Results = query.Skip(skip).Take(pageSize).ToList();

        return result;
    }
}
