using System;
using System.ComponentModel.DataAnnotations;

public class AppConfiguration
{
    public AISearchClient AISearchClient { get; set; }
    public OpenAIClient OpenAIClient { get; set; }
}

public class AISearchClient
{
    public required string Endpoint { get; set; }
    public required string IndexName { get; set; }
    public required string SemanticConfigName { get; set; }
    public required List<string> VectorFieldNames { get; set; }
    public required List<string> Fields { get; set; }
    public required int NearestNeighbours { get; set; }
    public required string EmbeddingClientName { get; set; }
    public required string ChatGptModelName { get; set; }
    public required string ChatGptKey { get; set; }
    public required string SystemPromptFileName { get; set; }
}

public class AISearchClientCredential
{
    public required string Key { get; set; }
}

public class OpenAIClient
{
    public required string Model { get; set; }
    public required string EmbeddingClientName { get; set; }
    public required string SystemPromptFileName { get; set; }
}