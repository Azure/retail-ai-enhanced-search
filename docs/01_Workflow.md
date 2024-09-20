# Overview

## Workflow

The infrastructure components get deployed with a Bicep template. The backend web API are in .NET code which run in the container app. This gets created with secrets which get auto-populated during deployment through the Bicep template.

![ContainerAppSecrets](../media/00_ContainerAppSecrets.PNG)

The spa folder contains the frontend react code.