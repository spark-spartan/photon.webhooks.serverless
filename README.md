# Photon Turnbased Webhooks Azure Functions (Server-less) Sample

## Summary
Implementation of [Photon Turnbased Webhooks Sample](https://github.com/exitgames/photon.webhooks.turnbased.waws) 
in Azure Functions Server-less approach. A **Photon Turnbased Webhooks** sample using [Azure Functions](https://azure.microsoft.com/en-gb/services/functions/), 
[Azure Blob & TableStorage](https://azure.microsoft.com/en-us/services/storage/) and [Notification Hub](https://azure.microsoft.com/en-gb/services/notification-hubs/).

## Requirments
* [Photon Realtime Account](https://www.photonengine.com/en/Realtime)
* [Visual Studio 2015 or Visual Studio Code and Azure Functions CLI](https://blogs.msdn.microsoft.com/appserviceteam/2016/12/01/running-azure-functions-locally-with-the-cli/) for local development
* [Azure Subsciption](https://azure.microsoft.com/en-gb/offers/ms-azr-0044p/) can get £150 free Azure Credit

## Run it locally
1) Load the .sln into Visual Studio 2015 (Visual Studio 2017 doesn't currently support Azure Functions) and hit Start. 
An Azure Function terminal should open with a list of URL endpoints. 

2) Open the code using Visual Studio Code

Then send a POST request e.g. `localhost:7071/api/hello-world/GameCreate`.
In the format `localhost:{port}/api/{appId}/Game{Controller}`

A [PostMan](https://www.getpostman.com/) Collection with examples is include in the Repo

## Deploy to Azure
Hit this link to create all infastructre required to run locally and in Azure.  
 
[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

Note - this will host the Functions in Azure as well as the infastructure, the 1st 1M function calls in azure are free, 
functions are charged at a per request basics so hosting the functions will not use credit if they aren't called.
