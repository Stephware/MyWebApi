using Microsoft.AspNetCore.Mvc;
using MyWebApi.Common;
using MyWebApi.Data;
using MyWebApi.DTOs;
using MyWebApi.Models;
using MyWebApi.Services;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] ProductQueryParameters queryParameters)
    {
        var result = _productService.GetAllProductsAsync(queryParameters);
        Response.Headers["X-Pagination"] = JsonSerialize(result.Pagination);
        return Ok(ApiResponse<PageResult<ProductDTO>>.SuccessResponse(result))
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var product = _productService.GetById(id);

            return Ok(ApiResponse<ProductDTO>.SuccessResponse(
                product,
                "Product retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object?>.FailResponse(ex.Message));
        }
    }

    // [HttpPost]
    // public IActionResult Create([FromBody] CreateProductDTO dto)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         var errors = ModelState.Values
    //             .SelectMany(v => v.Errors)
    //             .Select(e => e.ErrorMessage)
    //             .ToList();

    //         return BadRequest(ApiResponse<object?>.FailResponse(
    //             "Validation failed",
    //             errors));
    //     }

    //     if (!_store.Categories.ContainsKey(dto.CategoryId))
    //     {
    //         return BadRequest(ApiResponse<object?>.FailResponse(
    //             "Invalid category",
    //             new List<string> { $"Category with ID {dto.CategoryId} does not exist." }));
    //     }

    //     var id = _store.GetNextProductId();
    //     var product = new Product
    //     {
    //         Id = id,
    //         Name = dto.Name,
    //         Description = dto.Description,
    //         Sku = dto.Sku,
    //         Price = dto.Price,
    //         StockQuantity = dto.StockQuantity,
    //         CategoryId = dto.CategoryId,
    //         IsActive = true,
    //         Tags = dto.Tags,
    //         Created = DateTime.Now
    //     };

    //     _store.Products[id] = product;

    //     return CreatedAtAction(
    //         nameof(GetById),
    //         new { id = product.Id },
    //         ApiResponse<ProductDTO>.SuccessResponse(
    //             _productService.GetById(product.Id),
    //             "Product created successfully"));
    // }

    // [HttpPut("{id:int}")]
    // public IActionResult Update(int id, [FromBody] UpdateProductDTO dto)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         var errors = ModelState.Values
    //             .SelectMany(v => v.Errors)
    //             .Select(e => e.ErrorMessage)
    //             .ToList();

    //         return BadRequest(ApiResponse<object?>.FailResponse(
    //             "Validation failed",
    //             errors));
    //     }

    //     if (!_store.Products.TryGetValue(id, out var product))
    //     {
    //         return NotFound(ApiResponse<object?>.FailResponse(
    //             "Product not found",
    //             new List<string> { $"No product with ID {id} exists." }));
    //     }

    //     if (!_store.Categories.ContainsKey(dto.CategoryId))
    //     {
    //         return BadRequest(ApiResponse<object?>.FailResponse(
    //             "Invalid category",
    //             new List<string> { $"Category with ID {dto.CategoryId} does not exist." }));
    //     }

    //     product.Name = dto.Name;
    //     product.Description = dto.Description;
    //     product.Price = dto.Price;
    //     product.StockQuantity = dto.StockQuantity;
    //     product.CategoryId = dto.CategoryId;
    //     product.IsActive = dto.IsActive;
    //     product.Tags = dto.Tags;
    //     product.Updated = DateTime.Now;

    //     return Ok(ApiResponse<ProductDTO>.SuccessResponse(
    //         _productService.GetById(product.Id),
    //         "Product updated successfully"));
    // }

    // [HttpDelete("{id:int}")]
    // public IActionResult Delete(int id)
    // {
    //     if (!_store.Products.TryRemove(id, out _))
    //     {
    //         return NotFound(ApiResponse<object?>.FailResponse(
    //             "Product not found",
    //             new List<string> { $"No product with ID {id} exists." }));
    //     }

    //     return Ok(ApiResponse<object?>.SuccessResponse(
    //         null,
    //         "Product deleted successfully"));
    // }
}
