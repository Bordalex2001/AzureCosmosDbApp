using AzureCosmosDbApp.Data;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace AzureCosmosDbApp.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public CosmosDbService(string endpoint, string key, string databaseId, string containerId)
        {
            _cosmosClient = new CosmosClient(endpoint, key);
            _container = _cosmosClient.GetContainer(databaseId, containerId);
        }

        public async Task InitializeCosmosDbAsync()
        {
            DatabaseResponse databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync("EcommerceDB");
            await databaseResponse.Database.CreateContainerIfNotExistsAsync("Products", "/id", 400);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var query = _container.GetItemQueryIterator<Product>("SELECT * FROM Products");
            var results = new List<Product>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            try
            {
                ItemResponse<Product> response = await _container.ReadItemAsync<Product>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddProductAsync(Product product)
        {
            product.Id ??= Guid.NewGuid().ToString();
            await _container.CreateItemAsync(product, new PartitionKey(product.Id));
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _container.UpsertItemAsync(product, new PartitionKey(product.Id));
        }

        public async Task DeleteProductAsync(string id)
        {
            await _container.DeleteItemAsync<Product>(id, new PartitionKey(id));
        }
    }
}