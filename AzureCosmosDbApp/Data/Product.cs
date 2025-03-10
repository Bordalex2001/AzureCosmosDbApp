using Newtonsoft.Json;

namespace AzureCosmosDbApp.Data
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}