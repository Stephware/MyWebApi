using MyWebApi.Data;
using MyWebApi.Models;

namespace MyWebApi.Services;

public class ProductService : IProductService
{
    private readonly InMemoryDataStore _store;

    public ProductService(InMemoryDataStore store)
    {
        _store = store;
    }

    public Task<List<Product>> GetAllProductsAsync()
    {
        var products = _store.Products.Values
            .OrderBy(p => p.Id)
            .ToList();

        return Task.FromResult(products);
    }

    public Task<Product?> GetProductByIdAsync(int id)
    {
        _store.Products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }
}
