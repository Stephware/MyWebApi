using MyWebApi.Data;
using MyWebApi.DTOs;
using MyWebApi.Models;

namespace MyWebApi.Services;

public class ProductService : IProductService
{
    private readonly InMemoryDataStore _store;

    public ProductService(InMemoryDataStore store)
    {
        _store = store;
    }

    public List<ProductDTO> GetAll()
    {
        return _store.Products.Values
            .OrderBy(p => p.Id)
            .Select(ToDTO)
            .ToList();
    }

    public ProductDTO GetById(int id)
    {
        var product = FindOrThrow(id);
        return ToDTO(product);
    }

    private Product FindOrThrow(int id)
    {
        if (!_store.Products.TryGetValue(id, out var product))
        {
            throw new Exception("Product not found");
        }

        return product;
    }

    private ProductDTO ToDTO(Product product)
    {
        _store.Categories.TryGetValue(product.CategoryId, out var category);

        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Sku = product.Sku,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = category?.Name ?? "Unknown",
            Tags = product.Tags
        };
    }
}
