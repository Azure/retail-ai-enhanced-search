## What Program.CS file does

We inject the dependencies into the application using the Program.CS file. When the application starts it uses the following definitions to call endpoints we create in the Backend scripts.

![Calling Endpoints](../media/02_CallingEndpoints.PNG)

The endpoint definitions are taken from the [AppConfiguration class](/src/api/ProductSearchAPI/Models/AppConfiguration.cs)

![AppConfig](../media/02_AppConfig.PNG)

