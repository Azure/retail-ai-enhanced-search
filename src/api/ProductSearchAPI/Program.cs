using ProductSearchAPI;
using Microsoft.Extensions.Azure;
using Microsoft.AspNetCore.Mvc;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

var builder = WebApplication.CreateBuilder(args);
var config = new AppConfiguration();

builder.Configuration.GetSection("AppConfiguration").Bind(config);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddAzureOpenAIClient("OpenAI");
builder.Services.AddScoped<IProductSearchService, ProductSearchService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddClient<SearchClient, SearchClientOptions>((_, _, ServiceProvider) =>
    {
        return new SearchClient(new Uri(config.AISearchClient.Endpoint), config.AISearchClient.IndexName, new DefaultAzureCredential());
    });

    clientBuilder.AddClient<SearchIndexClient, SearchClientOptions>((_, _, ServiceProvider) =>
    {
        return new SearchIndexClient(new Uri(config.AISearchClient.Endpoint), new DefaultAzureCredential());
    });
});

builder.Services.AddCors(o => o.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

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

var app = builder.Build();

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

app.MapGet("/products", async Task<Results<Ok<List<Product>>, NotFound>> (
    [FromQuery(Name = "query")] string query,
    [FromServices] IProductSearchService productService
    ) =>
{
        List<Product> products = await productService.SearchProducts(
         query,
         config.AISearchClient.SemanticConfigName,
         config.OpenAIClient.EmbeddingClientName,
         config.AISearchClient.VectorFieldNames,
         config.OpenAIClient.Model,
         config.AISearchClient.NearestNeighbours,
         config.OpenAIClient.SystemPromptFileName,
         config.AISearchClient.Fields
     );

    if (products.Count <= 0)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(products);
});

app.MapGet("/stats", (
    [FromServices] IProductSearchService productService)
    =>
{
    Task<SearchServiceStatistics> stats = productService.GetSearchServiceStatistics();
    return stats.Result;
});

app.MapGet("/count", (
    [FromServices] IProductSearchService productService)
    =>
{
    Task<long> documentCount = productService.GetDocumentIndexCount();
    return documentCount.Result;
});

app.Run();
