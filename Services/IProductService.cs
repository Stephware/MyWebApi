using MyWebApi.Common;
using MyWebApi.DTOs;

namespace MyWebApi.Services;

public interface IProductService
{
    PagedResult<ProductDTO> GetAllProductsAsync(ProductQueryParameters queryParameters);
    ProductDTO GetById(int id);
}
