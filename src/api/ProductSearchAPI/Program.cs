/* 
#  Importing Libraries  #
*/

using ProductSearchAPI;
using Microsoft.Extensions.Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var config = new AppConfiguration();

/* 
#  Calling minimal API's #
*/

builder.Configuration.GetSection("AppConfiguration").Bind(config);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductSearchService, ProductSearchService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Creating the Azure Open AI client that talks to ChatGPT.
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddClient<Azure.AI.OpenAI.AzureOpenAIClient, Azure.AI.OpenAI.AzureOpenAIClientOptions>((_, _, ServiceProvider) =>
    {
        if (config.OpenAIClient != null && config.OpenAIClient.Deployment != null)
        {
            Console.WriteLine("Creating OpenAI client for chat GPT deployment: '{0}'", config.OpenAIClient.Deployment);
            return new Azure.AI.OpenAI.AzureOpenAIClient(new Uri(config.OpenAIClient.Endpoint), new DefaultAzureCredential(), null);
        } 
        else 
        {
            throw new Exception("Azure OpenAI client configuration is missing.");
        }
    });
});

// Creating the Azure Search client that talks to the Open AI Search service.
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddClient<SearchClient, SearchClientOptions>((_, _, ServiceProvider) =>
    {
        if (config.AISearchClient != null && config.AISearchClient.Endpoint != null && config.AISearchClient.IndexName != null)
        {
            Console.WriteLine("Creating search client with Endpoint: '{0}' and IndexName: '{1}'", config.AISearchClient.Endpoint, config.AISearchClient.IndexName);
            return new SearchClient(new Uri(config.AISearchClient.Endpoint), config.AISearchClient.IndexName, new DefaultAzureCredential());
        }
        else
        {
            throw new Exception("Search client configuration is missing.");
        }
    });

    clientBuilder.AddClient<SearchIndexClient, SearchClientOptions>((_, _, ServiceProvider) =>
    {
        if (config.AISearchClient != null && config.AISearchClient.Endpoint != null)
        {
            Console.WriteLine("Creating search index client with Endpoint: {0}", config.AISearchClient.Endpoint);
            return new SearchIndexClient(new Uri(config.AISearchClient.Endpoint), new DefaultAzureCredential());
        }
        else
        {
            throw new Exception("Search index client configuration is missing.");
        }
    });
});

builder.Services.AddCors(o => o.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

// Cross origin restrictions policy

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "OpenPolicy",
    policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy(name: "RestrictedPolicy",
    policy =>
    {
        policy.WithOrigins("https://server.com", "https://anotherserver.com")
        .AllowAnyHeader()
        .WithMethods("GET", "HEAD", "POST", "OPTIONS");
    });
});

// App getting build

var app = builder.Build();
app.Logger.LogInformation("Application Configuration: {0}", JsonSerializer.Serialize(config));

app.UseStatusCodePages(statusCodeHandlerApp =>
{
    statusCodeHandlerApp.Run(async httpContext =>
    {
        var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
        if (pds == null
          || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
        {
            await httpContext.Response.WriteAsync("Fallback: An error occurred.");
        }
    });
});

// If its Production enable Open API Swagger endpoint 
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async httpContext =>
        {
            var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
            if (pds == null
              || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
            {
                await httpContext.Response.WriteAsync("Fallback: An error occurred.");
            }
        });
    });
};

app.UseStatusCodePages();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/* Map to the products endpoint which maps to the products returned by the database. 
AI Search talks to database. We dont talk to database directly
*/
app.MapGet("/products", async Task<Results<Ok<List<Product>>, NotFound>> (
    [FromQuery(Name = "query")] string query,
    [FromServices] IProductSearchService productService
    ) =>
{
    List<Product> products = await productService.SearchProducts(
     query,
     config.AISearchClient.SemanticConfigName,
     config.AISearchClient.VectorFieldNames,
     config.OpenAIClient.Deployment,
     config.AISearchClient.NearestNeighbours,
     config.OpenAIClient.SystemPromptFile,
     config.AISearchClient.Fields
 );

    if (products.Count <= 0)
    {
        return TypedResults.NotFound();
    }

    var sortedProducts = products.OrderByDescending(p => p.Price).ToList();
    return TypedResults.Ok(sortedProducts);
});

app.Run();
