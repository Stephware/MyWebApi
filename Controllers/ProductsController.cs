using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1", Price = 10.99m },
        new Product { Id = 2, Name = "Product 2", Price = 19.99m },
        new Product { Id = 3, Name = "Product 3", Price = 5.99m }
    };

    [HttpGet]
    public IActionResult GetAll() => Ok(ApiResponse<List<Product>>.SuccessResponse(_products, "Products retrieved successfully"));

    [HttpGet ("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }
}