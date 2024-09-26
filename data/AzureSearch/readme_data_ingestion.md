# Data Ingestion And Azure Search Metadata creation

## objective
In this example, we will upload sample data into Cosmos DB and create an index in Azure AI Search to scan the data from Cosmos DB. While creating the Azure AI Search Index, we will also set up other associated resources such as Skillset, Indexer, Data Source, and Vectorizer. The below guidance will focus on the local development through VS Code.

## Prerequisites
1. You already have an Active Azure Subscription.
2. You have a python virtual environment where the required modules are already installed (requirements.txt). If you do not have the virtual environment, you can create one using the below command.

```bash
python -m venv < virtual environment name>
```
3. Install the packages that are required to run the script. You can install the packages using the below command.

```bash 
pip install -r requirements.txt
```

## Azure Service Requirements

    - Azure Cosmos DB
    - Azure AI Search
    - Azure Open AI

## configurations

We have two configurations files to update for Local Debugging.  

### .env file

- **COSMOS_ENDPOINT**="https://XXXXXXXXXXXXXX-cosmosdb.documents.azure.com/"
- **COSMOS_DATABASE**="catalogDb" - The name of the database in Cosmos DB
- **AZURE_SEARCH_ENDPOINT**="https://XXXXXX-search1.search.windows.net"
- **COSMOS_DB_CONNECTION_STRING**="ResourceId=/subscriptions/XXXXX/resourceGroups/XXXXXXXXXXXXXX/providers/Microsoft.DocumentDB/databaseAccounts/XXXXXX;Database=catalogDb;IdentityAuthType=AccessToken"
- **OPEN_AI_ENDPOINT**="https://XXXXXX-openai.openai.azure.com/"
- **OPEN_AI_EMBEDDING_DEPLOYMENT_NAME** = "embedding" - the deployment name of the Open AI Embedding model
- **AZURE_CLIENT_ID**="" - The user managed identity of the Azure agent who is running the script ( E.g. VM or Azure container app job). If we are running the script in the local environment, we can leave it blank.

### Search Config

`AzureSearch\config\config.json`

![alt text](.\images\config_image.png)

- **Cosmos config** : this portion contains the cosmos db configurations like cosmos db name, container name, partition key, and the fields which we want to index.
- **Search config** : This portion contains the Azure AI Search configurations like search service name, index name, indexer name, skillset name, and the fields which we want to index.
- **Open AI config** : This portion contains the Open AI configurations like open ai endpoint, model deployment name.

## RBAC permission required

-  **Cosmos DB**
    - [Cosmos DB Data Contributor Role](https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-setup-rbac#built-in-role-definitions) 
        - For the _Agent_ who is running the script. If we are running it from the Local environment, then our Object ID needs to have access to the Cosmos DB. If we are running it from Azure VM or Azure Container App, then the VM or Container App Managed Identity needs to have access to the Cosmos DB.
    - [Cosmos DB Data Reader Role](https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-setup-rbac#built-in-role-definitions)
        - For the _Azure AI Search managed Identity_
- **Azure AI Search**gary
- **Azure Open AI**
    - Azure AI Developer Role ( roleDefinitionId: '64702f94-c441-49e6-a78b-ef80e0188fee')
        - This is needed for the _Azure AI Search managed identity_ to access the Open AI Embedding model.
