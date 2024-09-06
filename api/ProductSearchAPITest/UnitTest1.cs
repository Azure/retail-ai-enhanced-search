using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using System.Text.Json;
using Azure.Search.Documents.Indexes.Models;

namespace ProductSearchAPI.Tests
{
    [TestClass]
    public class ProductSearchServiceTests
    {
        private Mock<ILogger<ProductSearchService>> _loggerMock;
        private Mock<SearchClient> _searchClientMock;
        private Mock<SearchIndexClient> _searchIndexClientMock;
        private Mock<AzureOpenAIClient> _openAIClientMock;
        private ProductSearchService _productSearchService;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ProductSearchService>>();
            _searchClientMock = new Mock<SearchClient>();
            _searchIndexClientMock = new Mock<SearchIndexClient>();
            _openAIClientMock = new Mock<AzureOpenAIClient>();

            _productSearchService = new ProductSearchService(
                _loggerMock.Object,
                _searchClientMock.Object,
                _searchIndexClientMock.Object,
                _openAIClientMock.Object,
                _fields = new List<string> { "Name", "Description", "Brand", "Type" };
            );
        }

        [TestMethod]
        public async Task SearchProducts_ValidQuery_ReturnsProducts()
        {
            // Arrange
            string queryText = "test query";
            string semanticConfigName = "semanticConfig";
            string embeddingClientName = "embeddingClient";
            string vectorFieldName = "vectorField";
            string chatGptDeploymentName = "chatGptDeployment";
            int nearestNeighbours = 3;

            var searchResultsMock = new Mock<SearchResults<Product>>();
            searchResultsMock.Setup(s => s.GetResultsAsync()).Returns((AsyncPageable<SearchResult<Product>>)GetMockSearchResults());

            // Act
            var result = await _productSearchService.SearchProducts(queryText, semanticConfigName, embeddingClientName, vectorFieldName, chatGptDeploymentName, nearestNeighbours);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task SearchProducts_EmptyQuery_ReturnsList()
        {
            // Arrange
            string queryText = "";
            string semanticConfigName = "semanticConfig";
            string embeddingClientName = "embeddingClient";
            string vectorFieldName = "vectorField";
            string chatGptDeploymentName = "chatGptDeployment";
            int nearestNeighbours = 3;

            // Act
            var result = await _productSearchService.SearchProducts(queryText, semanticConfigName, embeddingClientName, vectorFieldName, chatGptDeploymentName, nearestNeighbours);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        private static async IAsyncEnumerable<SearchResults<Product>> GetMockSearchResults()
        {
            var mockResults = SearchModelFactory.SearchResults<Product>(new[]
            {
                SearchModelFactory.SearchResult<Product>(new Product { Name = "a" }, 1.0, null),
                SearchModelFactory.SearchResult<Product>(new Product { Name = "b" }, 0.9, null),
            }, 2, null, null, null);
            yield return mockResults;
        }
    }
}
