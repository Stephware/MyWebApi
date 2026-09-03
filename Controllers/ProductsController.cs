using Microsoft.AspNetCore.Mvc;
using MyWebApi.Common;
using MyWebApi.DTOs;
using MyWebApi.Models;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Product 1", Price = 10.99m, IsActive = true, CreatedDate = DateTime.Now },
        new Product { Id = 2, Name = "Product 2", Price = 19.99m, IsActive = true, CreatedDate = DateTime.Now },
        new Product { Id = 3, Name = "Product 3", Price = 5.99m, IsActive = true, CreatedDate = DateTime.Now }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(ApiResponse<List<Product>>.SuccessResponse(
            _products,
            "Products retrieved successfully"));
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);

        if (product is null)
        {
            return NotFound(ApiResponse<Product>.ErrorResponse(
                "Product not found",
                new List<string> { $"No product with ID {id} exists." }));
        }

        return Ok(ApiResponse<Product>.SuccessResponse(
            product,
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

            return BadRequest(ApiResponse<object?>.ErrorResponse(
                "Validation failed",
                errors));
        }

        var newProduct = new Product
        {
            Id = _products.Count == 0 ? 1 : _products.Max(p => p.Id) + 1,
            Name = dto.Name,
            Description = dto.Description,
            Sku = dto.Sku,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            IsActive = true,
            Tags = dto.Tags,
            CreatedDate = DateTime.Now
        };

        _products.Add(newProduct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = newProduct.Id },
            ApiResponse<Product>.SuccessResponse(
                newProduct,
                "Product created successfully"));
    }
}
