# Overview

## Workflow

The infrastructure components get deployed with a **Bicep template**.
The **backend web API's** are in **.NET code** which run in the container app. This gets created with secrets which get auto-populated during deployment through the Bicep template.

![ContainerAppSecrets](../media/01_ContainerAppSecrets.PNG)

The spa folder contains the **frontend React code**.

The CosmosDB **Product** database gets created with a sample of 100 files and random images.
There is a **Product Catalog** inside the **Product** database. 

![Products](../media/01_Products.PNG)

The AI search components consists of **Index** that searches the cosmosDB for certain fields and a Semantic configuration for generic searches. 

|![SearchIndex](../media/01_SearchServiceIndex.PNG)| ![SemanticConfig](../media/01_SemanticConfig.PNG)|
| ------ | ---- |

There is also the **Indexer** which shows the date when the CosmosDB was indexed.

|![Indexer](../media/01_Indexer.PNG)| ![SemanticConfig](../media/01_IndexerDetails.PNG)|
| ------ | ---- |
