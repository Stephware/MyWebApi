namespace MyWebApi.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        <Product> GetProductByIdAsync(int id);
    }
}