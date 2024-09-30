using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using Azure.Search.Documents.Indexes.Models;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.Text.Json;

namespace ProductSearchAPI
{
    public interface IProductSearchService
    {
        Task<List<Product>> SearchProducts(string queryText, string semanticConfigName, IList<string> vectorFieldNames, string deploymentName, int nearestNeighbours, string systemPromptFilePath, List<string> fields);
    }

    public class ProductSearchService : IProductSearchService
    {
        private readonly ILogger<ProductSearchService> _logger;
        private readonly SearchClient _searchClient;
        private readonly SearchIndexClient _searchIndexClient;
        private readonly AzureOpenAIClient _openAIClient;

        public ProductSearchService(ILogger<ProductSearchService> logger, SearchClient searchClient, SearchIndexClient searchIndexClient, AzureOpenAIClient openAIClient)
        {
            _logger = logger;
            _searchClient = searchClient;
            _searchIndexClient = searchIndexClient;
            _openAIClient = openAIClient;
        }

        private async Task<System.ClientModel.ClientResult<ChatCompletion>> GetGPTChatResponse(string chatMessage, string systemPrompt, string chatDeploymentName)
        {
            ChatClient chatClient = _openAIClient.GetChatClient(chatDeploymentName);
            IList<ChatMessage> chatMessages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(chatMessage)
            };

            var options = new ChatCompletionOptions
            {
                Temperature = (float?)1.0,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
                MaxTokens = 256,
                ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
            };

            var chatResponse = await chatClient.CompleteChatAsync(chatMessages, options);
            return chatResponse;
        }

        public async Task<List<Product>> SearchProducts(string queryText, string semanticConfigName, IList<string> vectorFieldNames, string chatGptDeploymentName, int nearestNeighbours, string systemPromptFileName, List<string> fields)
        {
            string systemPrompt = string.Empty;
            string filter = string.Empty;
            List<Product> products = new List<Product>();

            try
            {
                systemPrompt = File.ReadAllText(systemPromptFileName);
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException($"Required file '{systemPromptFileName}' not found: {ex.Message}");
            }

            var chatGptResponse = await GetGPTChatResponse(queryText, systemPrompt, chatGptDeploymentName);
            if (chatGptResponse.Value.Content.Count <= 0 || String.IsNullOrEmpty(chatGptResponse.Value.Content.Last().Text))
            {
                _logger.LogInformation($"chatGptResponse is empty");
                return new List<Product>();
            }
            else
            {
                _logger.LogInformation($"chatGptResponse: {chatGptResponse.Value.Content[0]}");

                try
                {
                    AISearchFilter chatGptSearchFilter = new AISearchFilter();
                    if (chatGptResponse != null && !string.IsNullOrEmpty(chatGptResponse.Value.Content.Last().Text))
                    {
                        chatGptSearchFilter = JsonSerializer.Deserialize<AISearchFilter>(chatGptResponse.Value.Content.Last().Text);
                        _logger.LogInformation($"chatGptSearchFilter: {chatGptSearchFilter}");
                    }

                    if (chatGptSearchFilter != null && chatGptSearchFilter.Filters != null)
                    {
                        filter = chatGptSearchFilter.Filters.Trim();
                    }
                }
                catch (JsonException e)
                {
                    _logger.LogError($"Error parsing chatGptResponse: {e}");
                }

                var options = new SearchOptions
                {
                    VectorSearch = new()
                    {
                        Queries = {
                            new VectorizableTextQuery(queryText) {
                                KNearestNeighborsCount = nearestNeighbours,
                                Weight = 1,
                                Exhaustive = true,
                            }
                        }
                    },
                    SemanticSearch = new()
                    {
                        SemanticConfigurationName = semanticConfigName,
                        QueryCaption = new(QueryCaptionType.Extractive),
                        QueryAnswer = new(QueryAnswerType.Extractive),
                    },
                    QueryType = SearchQueryType.Semantic
                };

                if (filter != null || filter != string.Empty || filter != "")
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        options.Filter = filter.ToLower();
                    }
                }

                foreach (string vectorFieldName in vectorFieldNames)
                {
                    options.VectorSearch.Queries[0].Fields.Add(vectorFieldName);
                }

                foreach (string field in fields)
                {
                    options.SearchFields.Add(field);
                }

                _logger.LogInformation($"Search options: {options}");

                try
                {
                    var response = await _searchClient.SearchAsync<Product>(
                        queryText,
                        options);

                    int documentCount = 0;

                    await foreach (SearchResult<Product> result in response.Value.GetResultsAsync())
                    {
                        documentCount++;
                        Product product = result.Document;
                        products.Add(product);
                        var serializedProduct = JsonSerializer.Serialize(product);
                        _logger.LogInformation($"Product: {serializedProduct}");
                    }

                    _logger.LogInformation($"Found '{documentCount}' documents");
                    return products;

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error searching products: {ex.Message}");
                }

                return new List<Product>();
            }
        }
    }
}
