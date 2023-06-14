namespace TemplateCQRS.WebApp.Data.Pagination;

public class PaginationLinks
{
    private string back = "Anterior";
    private string first = "Primera";
    private string page = "Página";
    private string last = "Última";
    private string next = "Siguiente";

    public List<PagingLink> CreatePaginationLinks<T>(PagedResult<T> pagedList, int paginationSize)
    {
        List<PagingLink> links = new();

        PagingLink newPage = new(pagedList.CurrentPage - 1, pagedList.HasPrevious, back, $"{back} {page}");
        links.Add(newPage);

        newPage = new PagingLink(1, pagedList.HasPrevious, "<<", $"{first} {page}");
        links.Add(newPage);

        for (var pageNumber = 1; pageNumber <= pagedList.TotalPages; pageNumber++)
        {
            newPage = new PagingLink(pageNumber, true, pageNumber.ToString(), $"{page} {pageNumber}");

            if (pageNumber == pagedList.CurrentPage)
            {
                newPage.Active = pagedList.CurrentPage == pageNumber;
            }

            if (pageNumber >= pagedList.CurrentPage && pageNumber <= pagedList.CurrentPage + paginationSize)
            {
                links.Add(newPage);
            }

            else if (pageNumber >= pagedList.CurrentPage - paginationSize && pageNumber <= pagedList.CurrentPage &&
                     pageNumber + paginationSize >= pagedList.TotalPages)
            {
                links.Add(newPage);
            }
        }

        // Show the last page available
        newPage = new PagingLink(pagedList.TotalPages, pagedList.HasNext, ">>", $"{last} {page}");
        links.Add(newPage);

        newPage = new PagingLink(pagedList.CurrentPage + 1, pagedList.HasNext, next, $"{next} {page}");
        links.Add(newPage);

        return links;
    }
}
