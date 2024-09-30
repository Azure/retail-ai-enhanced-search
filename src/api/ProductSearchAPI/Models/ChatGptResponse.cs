using System.Text.Json.Serialization;

namespace ProductSearchAPI
{
    public class AISearchFilter
    {
        [JsonPropertyName("filters")]
        public string? Filters { get; set; }
    }
}
