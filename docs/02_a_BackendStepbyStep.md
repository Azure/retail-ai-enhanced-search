### Step 1
It first creates a Cosmos container, database and a database partition key(In this sample the **id** field is used).
> :bulb: **Tip:** Sample dataset should print the following result:
"Getting Database: catalogDB", "Getting client for container: products"

### Step 2
It then uploads the data in the CSV file to the newly created Cosmos database.
> :bulb: **Tip:**  Sample dataset should print the following result:
"Uploading Data...", "Inserting product ID: {each product ID should get displayed here} to Cosmos DB","Product {each product ID should get displayed here} uploaded to Cosmos DB". It will iterate through all 101 items in the CSV file

### Step 3
The Index definition is created based on the fields mentioned in the config.json file. We also set variables required for [Vectorizer](https://learn.microsoft.com/azure/search/vector-search-how-to-configure-vectorizer) and [Semantic Configurations](https://learn.microsoft.com/azure/search/semantic-how-to-configure?tabs=portal) based on the configurations mentioned in the config file.
> :bulb: **Tip:**  Sample dataset should print the following result:
"Setting Indexer Variables..."

### Step 4
A service endpoint is created pointing the database container name. It uses the [search service managed identity connection string](https://learn.microsoft.com/azure/search/search-howto-index-cosmosdb#supported-credentials-and-connection-strings) to connect to CosmosDB
> :bulb: **Tip:**  Sample dataset should print the following result:
"Creating Search Client with Endpoint: <endpointname>", "Data Source created successfully."

### Step 5
Here is where we leverage te variables set in Step 4 and create the AI [Search Index](https://learn.microsoft.com/azure/search/search-what-is-an-index). 
> :bulb: **Tip:**  Sample dataset should print the following result:
"Search Index created successfully"

### Step 6
OpenAI’s text embeddings measure the relatedness of text strings. Embeddings are commonly used for:

- **Search** (where results are ranked by relevance to a query string)
- **Clustering** (where text strings are grouped by similarity)
- **Recommendations** (where items with related text strings are recommended)
- **Anomaly detection** (where outliers with little relatedness are identified)
- **Diversity measurement** (where similarity distributions are analyzed)
- **Classification** (where text strings are classified by their most similar label)
In this step we configure the [OpenAI Embedding Skillset](https://learn.microsoft.com/azure/search/cognitive-search-skill-azure-openai-embedding).

> :bulb: **Tip:**  Sample dataset should print the following result:
"Creating OpenAI Embedding Skillset","Creating the Skillset", "Skillset created successfully"

### Step 7
Finally we create the [Indexer](https://learn.microsoft.com/azure/search/search-indexer-overview) and run it.The indexer is used to scan the data from the Cosmos DB and push the data to the Azure AI Search Index.
> :bulb: **Tip:**  Sample dataset should print the following result:
"Creating the indexer.", "Indexer created successfully."
On an initial run, when the index is empty, an indexer will read in all of the data provided in the table or container. On subsequent runs, the indexer can usually detect and retrieve just the data that has changed. Since we are using Azure CosmosDB we have already enabled the change detection. This is to enable the different stages of indexing.
![alt text](../media/01_indexer-stages.png)
