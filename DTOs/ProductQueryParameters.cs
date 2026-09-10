namespace MyWebApi.DTOs;

public class ProductQueryParameters
{
    public const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : Math.Min(value, MaxPageSize);
    }

    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? InStockOnly { get; set; }
    public bool? IsActive { get; set; }
    public string? SortBy { get; set; } = "name";
    public bool SortDescending { get; set; } = false;
}
