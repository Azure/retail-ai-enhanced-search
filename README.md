# e-Retail-AI-Enhanced-Search

## Introduction

Highly relevant product discovery by its consumers is critical for any retailer (direct or indirect). This is the first step to drive sales (their top line), as product discovery converts into a sale through multiple channels, offers, promotions etc.

Every major retail store has an online presence in the e-retail space, through their own portals or through other e-retail collaborations, where their consumers can search for products, browse through catalogs etc.

Advent of GenAI LLM is revolutionizing the way product search and discovery works. Powerful LLMs identify a consumer’s intent with higher accuracy resulting in highly relevant search results. Over the past years, several customers have started incorporating AI enhanced search, either through existing search engines or building interactive chatbots.

The following extract from an [IDC Research Report](https://www.idc.com/getdoc.jsp?containerId=US51940624&pageType=PRINTFRIENDLY) describes the use-case in detail.

![User Experience](./media/00_Introduction.png)

## User Experience

![User Experience](./media/00_User_Experience.png)

Product discovery is a crucial aspect of the e-retail experience, enabling shoppers to find the items they are looking for and exposing them to new products they may want. Effective product discovery can significantly enhance customer satisfaction, increase conversion rates, and boost customer loyalty.

- Key Elements of Product Discovery:Search Functionality: Advanced search features, including text and visual search, help customers find products quickly and accurately.
- Personalized Recommendations: Using data analytics, retailers can offer personalized product recommendations based on individual customer preferences and behaviours.
- Navigation and Filters: Well-organized navigation and filtering options make it easier for customers to browse and discover products.
- Product Catalogue Enrichment: With LLM model, E-retailer can enrich their product catalogue for search accuracy and increasing semantic relevance.

Overall, the e-retail industry is rapidly evolving, with product discovery playing a pivotal role in shaping the online shopping experience. Retailers must continuously innovate and adapt to meet the changing needs of consumers.

## Solution Architecture

![Solution Architecture](./media/00_Solution_Architecture.png)

Components of the solution are as follows:

- Azure Cosmos DB is a globally distributed database service. It is well suited for low latency applications like search engines.
- AI Search is a cloud solution that provides a rich search experience with key word and vector store capabilities over private, heterogeneous content in web, mobile, and enterprise applications.
- Azure App Service - Web Apps hosts web applications allowing auto scale and high availability without having to manage infrastructure.
- Azure OpenAI Service provides REST API access to OpenAI's powerful language models including Embeddings model series. Users can access the service through REST APIs, Python SDK, or our web-based interface in the Azure OpenAI Studio.  
- Azure Functions is a serverless solution that makes it possible for you to write less code, maintain less infrastructure, and save on costs.
- Azure Content Safety is a service that helps you detect and filter harmful user-generated and AI-generated content in your applications and services. Content Safety includes text and image detection to find content that is offensive, risky, or undesirable, such as profanity, adult content, gore, violence, hate speech, and more.

## Getting Started

- Prerequisites
  - Azure Subscription
  - [Azure PowerShell](https://docs.microsoft.com/en-us/powershell/azure/install-az-ps)
  - Bash shell
  - [Git](https://git-scm.com/downloads)
  - [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
  - [VS Code](https://code.visualstudio.com/download)
  - [Node.js](https://nodejs.org/en/download/package-manager)
  - [Dotnet 8.0 Core](https://dotnet.microsoft.com/download)
  - Virtual Network with subnet configurations for resources
- [Key Concepts to Understand](./docs/01_Concepts.md)
- [Solution Quickstart](./docs/02_Solution_Quickstart.md)

## Local Development

1. Clone this repository to your local machine
2. Deploy base infrastructure using AI-Hub ARM templates
3. Compile & run the back-end API
   - Navigate to the '/api/ProductSearchAPI' folder
   - Rename ./api/ProductSearchAPI/appsettings.template to appsettings.json
   - Enter the required values in appsettings.json from the AI-Hib deployment output
   - Run `$ dotnet run`
   - The API will be running on `http://localhost:60871`
   - Logs can be viewed live in the VS Code Terminal
4. Open a new Terminal within VSCode and navigate to the '/spa' folder
   - Create a file named `/.env` & add a line with the following text
     - `REACT_APP_API_URL=http://localhost:5173`
   - Run `$ npm install`
   - Run `$ npm run dev`
   - Access the React application in a browser at `http://localhost:5173`
5. Test the application by searching for a product in the React app or access the Swagger UI at `http://localhost:60871/swagger`

## PoC Environment Detailed Guide

Additional explanation for each configurations and usage pattern PoC environment is in [PoC Environment Guide](./docs/03_PoC_Environment_Guide.md) document.
> Gary, Varmar, Sam, Arun and Chris until 27th, September

## Production Environment Detailed Guide

You can bring your own data or you can configure AI Search with your existing data sources such as Azure Cosmos DB, Azure SQL Database or existing Storage Account.

Based on your data character / usage / pattern, you need to enable and configure necessary features & parameters correctly.

Additional detail guidance is in [Production Environment Guide](./docs/04_PRD_Environment_Guide.md) document.
> Gary, Varmar, Sam, Arun and Chris for Guide Document until 27th, September

## Other Resources

- [Azure Cosmos DB - Database for the AI Era](https://learn.microsoft.com/en-us/azure/cosmos-db/introduction)
- [Understand embeddings in Azure OpenAI Service](https://learn.microsoft.com/en-us/azure/openai-service/understand-embeddings)
- [Vectors in Azure AI Search](https://learn.microsoft.com/en-us/azure/search/vector-search-concept-intro)
- [Integrated data chunking and embedding in Azure AI Search](https://learn.microsoft.com/en-us/azure/search/vector-search-integrated-vectorization)
- [Semantic ranking in Azure AI Search](https://learn.microsoft.com/en-us/azure/search/semantic-ranking-intro)
- [AI enrichment in Azure AI Search](https://learn.microsoft.com/en-us/azure/search/cognitive-search-concept-intro)
- [Skillset concepts in Azure AI Search](https://learn.microsoft.com/en-us/azure/search/cognitive-search-concept-skillset)
