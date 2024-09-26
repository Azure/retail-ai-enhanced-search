[Work In Progress]

# Local Development

- Prerequisites
  - Azure Subscription
  -[Azure PowerShell](https://docs.microsoft.com/en-us/powershell/azure/install-az-ps)
  - Bash shell
  - [Git](https://git-scm.com/downloads)
  - [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
  - [VS Code](https://code.visualstudio.com/download)
  - [Node.js](https://nodejs.org/en/download/package-manager)
  - [Dotnet 8.0 Core](https://dotnet.microsoft.com/download)
  - [Key Concepts to Understand](./docs/01_Concepts.md)
  - [Solution Quickstart](./docs/02_Solution_Quickstart.md)

1. Clone this repository to your local machine
2. Deploy base infrastructure using AI-Hub ARM templates
3. Compile & run the back-end API
   - Navigate to the '/api/ProductSearchAPI' folder
   - Rename ./api/ProductSearchAPI/appsettings.template to appsettings.json
   - Enter the required values in appsettings.json from the AI-Hib deployment output
   - Run `$ dotnet run`
   - The API will be running on `http://localhost:60871`
   - Logs can be viewed live in the VS Code Terminal
   - Test the API by searching for a product using the Swagger UI at `http://localhost:60871/swagger`
4. Open a new Terminal within VSCode and navigate to the '/spa' folder
   - Create a file named `/.env` & add a line with the following text
     - `REACT_APP_API_URL=http://localhost:5173`
   - Run `$ npm install`
   - Run `$ npm run dev`
   - Access the React application in a browser at `http://localhost:5173`
