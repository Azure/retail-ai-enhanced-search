﻿using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using Azure.Search.Documents.Indexes.Models;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Embeddings;
using OpenAI.Chat;
using System.Text.Json;

namespace ProductSearchAPI
{
    public interface IProductSearchService
    {
        Task<List<Product>> SearchProducts(string queryText, string semanticConfigName, string embeddingClientName, string vectorFieldName, string deploymentName, int nearestNeighbours, string systemPromptFilePath, List<string> fields);
        Task<SearchServiceStatistics> GetSearchServiceStatistics();
        Task<long> GetDocumentIndexCount();
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

        private ReadOnlyMemory<float> GetEmbeddings(string input, string embeddingClientName)
        {
            try
            {
                EmbeddingClient embeddingClient = _openAIClient.GetEmbeddingClient(embeddingClientName);
                Embedding embedding = embeddingClient.GenerateEmbedding(input);
                return embedding.Vector;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating embeddings for input: {input}", input);
                return null;
            }
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
                Temperature = (float?)0.7,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            var chatResponse = await chatClient.CompleteChatAsync(chatMessages, options);
            return chatResponse;
        }

        public async Task<List<Product>> SearchProducts(string queryText, string semanticConfigName, string embeddingClientName, string vectorFieldName, string chatGptDeploymentName, int nearestNeighbours, string systemPromptFileName, List<string> fields)
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

            ReadOnlyMemory<float> vectorizedResult = GetEmbeddings(queryText, embeddingClientName);

            if (vectorizedResult.IsEmpty || String.IsNullOrEmpty(queryText))
            {
                return new List<Product>();
            }

            var chatGptResponse = await GetGPTChatResponse(queryText, systemPrompt, chatGptDeploymentName);

            if (chatGptResponse.Value.Content.Count <= 0 || String.IsNullOrEmpty(chatGptResponse.Value.Content[0].Text))
            {
                _logger.LogInformation($"chatGptResponse is empty");
                return new List<Product>();
            }
            else
            {
                _logger.LogInformation($"chatGptResponse: {chatGptResponse.Value.Content[0]}");

                try
                {
                    AISearchFilter chatGptSearchFilter = JsonSerializer.Deserialize<AISearchFilter>(
                        chatGptResponse.Value.Content[0].Text
                        );
                    filter = chatGptSearchFilter.Filter.Trim();
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
                            new VectorizedQuery(vectorizedResult) {
                                KNearestNeighborsCount = nearestNeighbours,
                                Fields = { vectorFieldName }
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

                if (filter != null)
                {
                    options.Filter = filter;
                }

                foreach (string field in fields)
                {
                    options.SearchFields.Add(field);
                }

                SearchResults<Product> response = await _searchClient.SearchAsync<Product>(
                    queryText,
                    options);

                _logger.LogInformation($"response count: {response.TotalCount}");

                int documentCount = 0;
                await foreach (SearchResult<Product> result in response.GetResultsAsync())
                {
                    documentCount++;
                    Product doc = result.Document;
                    products.Add(doc);
                    _logger.LogInformation($"Name: {doc.Name}");
                }

                _logger.LogInformation($"Found '{documentCount}' documents");
                return products;
            }
        }

        public async Task<SearchServiceStatistics> GetSearchServiceStatistics()
        {
            Response<SearchServiceStatistics> stats = await _searchIndexClient.GetServiceStatisticsAsync();
            return stats;
        }

        public async Task<long> GetDocumentIndexCount()
        {
            Response<long> count = await _searchClient.GetDocumentCountAsync();
            return count;
        }
    }
}
