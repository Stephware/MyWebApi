using MyWebApi.Data;
using MyWebApi.Models;

namespace MyWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly InMemoryDataStore _store;

        public ProductService(InMemoryDataStore store)
        {
            _store = store;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = _store.Products.Value.ToList();

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = FindOrThrow(id);
            return product;
        }

        private Product FindOrThrow(int id)
        {
            if (!_store.Products.TryGetValue(id, out var product))
            {
                throw new Exception($"Product with ID {id} not found.");
            }
            return product;
        }
    }
}