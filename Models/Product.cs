namespace MyWebApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool InStock => StockQuantity > 0;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List <tags> Tags { get; set; } = new List<tags>();
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? Updated { get; set; }
}
