using AzureCosmosDbApp.Data;

namespace AzureCosmosDbApp.Services
{
    public interface ICosmosDbService
    {
        Task InitializeCosmosDbAsync();
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string id);
    }
}
