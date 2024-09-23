using System.Text.Json.Serialization;

namespace ProductSearchAPI
{
    public class Product
    {
        [JsonPropertyName("id")]        
        public string? Id { get; set; }
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        [JsonPropertyName("brand")]
        public string? Brand { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("price")]
        public double? Price { get; set; }
        [JsonPropertyName("imageName")]
        public string? ImageName { get; set; }
    }
}
