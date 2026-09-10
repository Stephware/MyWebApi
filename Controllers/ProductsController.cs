using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Common;
using MyWebApi.DTOs;
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

        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.Pagination);

        return Ok(ApiResponse<PagedResult<ProductDTO>>.SuccessResponse(
            result,
            "Products retrieved successfully"));
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
}
