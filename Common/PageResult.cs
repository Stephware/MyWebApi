namespace MyWebApi.Common;

public class PaginationMetadata
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public PaginationMetadata Pagination { get; set; }

    public PageResult(List<T> items, int pageNumber, int pageSize, int totalCount)
    {
        return new PagedResult<T>
        {
            Items = items,
            Pagination = new PaginationMetadata
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }   
    }
}