using MyWebApi.DTOs;

namespace MyWebApi.Services;

public interface IProductService
{
    List<ProductDTO> GetAll();
    ProductDTO GetById(int id);
}
