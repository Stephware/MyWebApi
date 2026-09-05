using Microsoft.AspNetCore.Mvc;
using MyWebApi.Common;
using MyWebApi.Data;
using MyWebApi.DTOs;
using MyWebApi.Models;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly InMemoryDataStore _store;

    public ProductsController(InMemoryDataStore store)
    {
        _store = store;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _store.Products.Values
            .OrderBy(p => p.Id)
            .Select(ToDto)
            .ToList();

        return Ok(ApiResponse<List<ProductDTO>>.SuccessResponse(
            products,
            "Products retrieved successfully"));
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        if (!_store.Products.TryGetValue(id, out var product))
        {
            return NotFound(ApiResponse<object?>.FailResponse(
                "Product not found",
                new List<string> { $"No product with ID {id} exists." }));
        }

        return Ok(ApiResponse<ProductDTO>.SuccessResponse(
            ToDto(product),
            "Product retrieved successfully"));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateProductDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object?>.FailResponse(
                "Validation failed",
                errors));
        }

        if (!_store.Categories.ContainsKey(dto.CategoryId))
        {
            return BadRequest(ApiResponse<object?>.FailResponse(
                "Invalid category",
                new List<string> { $"Category with ID {dto.CategoryId} does not exist." }));
        }

        var id = _store.GetNextProductId();
        var product = new Product
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            Sku = dto.Sku,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            IsActive = true,
            Tags = dto.Tags,
            Created = DateTime.Now
        };

        _store.Products[id] = product;

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            ApiResponse<ProductDTO>.SuccessResponse(
                ToDto(product),
                "Product created successfully"));
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] UpdateProductDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object?>.FailResponse(
                "Validation failed",
                errors));
        }

        if (!_store.Products.TryGetValue(id, out var product))
        {
            return NotFound(ApiResponse<object?>.FailResponse(
                "Product not found",
                new List<string> { $"No product with ID {id} exists." }));
        }

        if (!_store.Categories.ContainsKey(dto.CategoryId))
        {
            return BadRequest(ApiResponse<object?>.FailResponse(
                "Invalid category",
                new List<string> { $"Category with ID {dto.CategoryId} does not exist." }));
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;
        product.IsActive = dto.IsActive;
        product.Tags = dto.Tags;
        product.Updated = DateTime.Now;

        return Ok(ApiResponse<ProductDTO>.SuccessResponse(
            ToDto(product),
            "Product updated successfully"));
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!_store.Products.TryRemove(id, out _))
        {
            return NotFound(ApiResponse<object?>.FailResponse(
                "Product not found",
                new List<string> { $"No product with ID {id} exists." }));
        }

        return Ok(ApiResponse<object?>.SuccessResponse(
            null,
            "Product deleted successfully"));
    }

    private ProductDTO ToDto(Product product)
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
            CategoryName = category?.Name ?? string.Empty,
            Tags = product.Tags
        };
    }
}
