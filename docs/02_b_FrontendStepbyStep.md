## What Program.CS file does

We inject the dependencies into the application using the Program.CS file. When the application starts it uses the following definitions to call endpoints we create in the Backend scripts.

|<img src='../media/02_CallingEndpoints.PNG' width='720' height='300'>|
| ------ |

The endpoint definitions are taken from the [AppConfiguration class](/src/api/ProductSearchAPI/Models/AppConfiguration.cs). Essentially what's passed in to the environment variables of the container app gets bound to these fields.

![alt text](../media/02_ContainerEnvironmentVariables.PNG)
|<img src='../media/02_AppConfig.PNG' width='500' height='400'>|
| ------ |

