# Overview

Under the SRC folder you will find **[api](../src/api/)** and **[data](../src/data/)** which contains the application and data code. The repo uses the [products.csv](../data/AzureSearch/data/products.csv) as sample data. It looks as follows

<img src='/media/01_Productsample.PNG' width='950' height='150'>

Using this sample data a search index is created on the following fields

![SearchIndex](../media/01_SearchIndexStructure.PNG)

Based on the above structure various fields are called to integrate filtering, sorting, vectorization capabilities and dedicate how the search results will look like. This is done using [Search Index](https://learn.microsoft.com/azure/search/search-what-is-an-index), [Indexer](https://learn.microsoft.com/azure/search/search-indexer-overview) & [Vector Store](https://learn.microsoft.com/azure/search/vector-store)capabilities of Azure AI Search. This is leveraged for creating an [Open AI Embedding](https://learn.microsoft.com/azure/search/cognitive-search-skill-azure-openai-embedding)

<img src='/media/01_SearchFields.PNG' width='850' height='550'>

These configurations get called in the [createIndex.py](/src/data/AzureSearch/createIndex.py)
It creates the following resources
1) A Cosmos Endpoint
2) A Cosmos Database with partitioned data
3) A Cosmos Connection String
4) An Azure AI Search Index
5) An Azure AI Search Indexer
6) An Azure Search Endpoint
7) An Open AI Endpoint
8) An Open AI Embedding Skillset 
9) Recreating the indexer

The code is executed using a [Default Azure Credential](https://learn.microsoft.com/python/api/azure-identity/azure.identity.defaultazurecredential?view=azure-python) from Azure Identity. 

Step 1: It first creates a Cosmos container, database and a database partition key(In this sample the **id** field is used).
> :bulb: **Tip:** Sample dataset should print the following result:
"Getting Database: catalogDB", "Getting client for container: products"

Step 2: It then uploads the data in the CSV file to the newly created Cosmos database 
> :bulb: **Tip:**  Sample dataset should print the following result:
"Uploading Data...", "Inserting product ID: {each product ID should get displayed here} to Cosmos DB","Product {each product ID should get displayed here} uploaded to Cosmos DB". It will iterate through all 101 items in the CSV file

The CosmosDB **catalogDb** database gets created with a sample of 101 files and random images.
This resides under the **products** container within Cosmos DB. Cosmos DB version azure-cosmos==4.7.0


## Workflow

The infrastructure components get deployed with a **Bicep template**.
The **backend web API's** are in **.NET code** which run in the container app. This gets created with secrets which get auto-populated during deployment through the Bicep template.

![ContainerAppSecrets](../media/01_ContainerAppSecrets.PNG)

The spa folder contains the **frontend React code**. This runs as a **static web application**. It has an API connection to the container app. No image search functionality

![ConnectiontoConaitnerApp](../media/01_ConnectionContainerApp.PNG)




The AI search components consists of **Index** that searches the cosmosDB for certain fields and a Semantic configuration for generic searches. 

|![SearchIndex](../media/01_SearchServiceIndex.PNG)| ![SemanticConfig](../media/01_SemanticConfig.PNG)|
| ------ | ---- |

There is also the **Indexer** which shows the date when the CosmosDB was indexed.

|![Indexer](../media/01_Indexer.PNG)| ![SemanticConfig](../media/01_IndexerDetails.PNG)|
| ------ | ---- |

**APIM** is public facing. The frontend is reactive in its layout. Furthermore it has paging and filters which let's you perform key value search. It can also cater to interactive search.

![KeyValueSearch](../media/01_Keyvaluesearch.png)
![InteractiveSearch](../media/01_InteractiveSearch.png)