using MyWebApi.Common;
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

    public PagedResult<ProductDTO> GetAllProductsAsync(ProductQueryParameters queryParameters)
    {
        var query = ApplyFilterSearchSort(queryParameters);
        var totalCount = query.Count;

        var pageItems = query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .Select(ToDTO)
            .ToList();

        return PagedResult<ProductDTO>.Create(
            pageItems,
            queryParameters.PageNumber,
            queryParameters.PageSize,
            totalCount);
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

    private List<Product> ApplyFilterSearchSort(ProductQueryParameters p)
    {
        var query = _store.Products.Values.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(p.Search))
        {
            var term = p.Search.Trim();
            query = query.Where(x =>
                x.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.Description.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.Sku.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        if (p.CategoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == p.CategoryId.Value);
        }

        if (p.MinPrice.HasValue)
        {
            query = query.Where(x => x.Price >= p.MinPrice.Value);
        }

        if (p.MaxPrice.HasValue)
        {
            query = query.Where(x => x.Price <= p.MaxPrice.Value);
        }

        if (p.InStockOnly == true)
        {
            query = query.Where(x => x.StockQuantity > 0);
        }

        if (p.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == p.IsActive.Value);
        }

        query = p.SortBy?.ToLowerInvariant() switch
        {
            "price" => p.SortDescending ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
            "stockquantity" => p.SortDescending ? query.OrderByDescending(x => x.StockQuantity) : query.OrderBy(x => x.StockQuantity),
            "created" => p.SortDescending ? query.OrderByDescending(x => x.Created) : query.OrderBy(x => x.Created),
            _ => p.SortDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
        };

        return query.ToList();
    }
}
