
@description('Location to deploy the resources')
param location string = resourceGroup().location

@description('Prefix for the environment name when creating the resources in this deployment.')
param environmentNamePrefix string

@description('Prefix for the name of the application, workload, or service that the resource is a part of.')
param ProjectOrApplicationNamePrefix string = 'showcase'

@description('Prefix for the name of the application instance.')
param ProjectOrApplicationInstanceNamePrefix string = '01'

@description('Runtime for the function worker.')
@allowed([
  'dotnet'
  'node'
  'python'
  'java'
  'dotnet-isolated'
])
param functionWorkerRuntime string 

@description('Name of the function app.')
param FunctionAppName string

@description('Specifies the operating system used for the Azure Function hosting plan.')
@allowed([
  'Windows'
  'Linux'
])
param functionPlanOS string


var applicationRegionPrefix = substring(location, 0, 2)

var resourceNameSuffix = '${environmentNamePrefix}-${applicationRegionPrefix}-${ProjectOrApplicationInstanceNamePrefix}'

var hostingPlanName = 'asp-${ProjectOrApplicationNamePrefix}-${resourceNameSuffix}'

var storageAccountNamePrefix = 'st-${ProjectOrApplicationNamePrefix}-${resourceNameSuffix}'

var storageAccountFullName = toLower(replace(storageAccountNamePrefix, '-', ''))

var storageAccountName = length(storageAccountFullName) > 23 ? substring(storageAccountFullName,0,23) : storageAccountFullName

var isReserved = ((functionPlanOS == 'Linux') ? true : false)

var functionAppkind = (isReserved ? 'functionapp,linux' : 'functionapp')


var functionAppName = 'func-${ProjectOrApplicationNamePrefix}-${FunctionAppName}-${resourceNameSuffix}'

var AIServiceFormRecognizerName = 'fcog-${ProjectOrApplicationNamePrefix}-${FunctionAppName}-${resourceNameSuffix}' 

var emailAttachmentsLogicAppName = 'logic-${ProjectOrApplicationNamePrefix}-${FunctionAppName}-${resourceNameSuffix}'
var azureBlobWebConnectionsName  = 'wcn-${ProjectOrApplicationNamePrefix}-azureblob-${resourceNameSuffix}' 
var office365WebConnectionsName  = 'wcn-${ProjectOrApplicationNamePrefix}-office365-${resourceNameSuffix}' 

module storageAccountResource './templates/storage-account.bicep' ={
  name: storageAccountName
  params: { 
  storageAccountName:storageAccountName
  storageAccountType:'Standard_LRS'
  location:location
  }
}


module AIServiceNameFormRecognizer './templates/azure-ai-services.bicep'={
name:AIServiceFormRecognizerName
params:{
  location:location
  AIServiceName:AIServiceFormRecognizerName
  sku:'S0'
  kind:'FormRecognizer'
}

}

module containersBlobServices './templates/blob-services.bicep' ={
  name:'containersBlobServices'
  params:{
    storageAccountName:storageAccountName
    storagecontainersNames:[
         'raw-customer-invoice-email-attachments'
         'json-customer-invoice-email-attachments'
    ]
  }
}

module webConnectionsAzureBlob './templates/web-connections.bicep'={
  name:azureBlobWebConnectionsName
  params:{
    location:location
    webConnectionsName:azureBlobWebConnectionsName
    webConnectionsExternalid:'subscriptions/${subscription().subscriptionId}/providers/Microsoft.Web/locations/${location}/managedApis/azureblob'
  }
}


module webConnectionsOffice365 './templates/web-connections.bicep'={
  name:office365WebConnectionsName
  params:{
    location:location
    webConnectionsName:office365WebConnectionsName
    webConnectionsExternalid:'subscriptions/${subscription().subscriptionId}/providers/Microsoft.Web/locations/${location}/managedApis/office365'
  }
}


module workflowsEmailAttachmentsIntoBlobStorageLogicApp './templates/email-attachments-into-blob-storage-logic-app.bicep'={
  name:emailAttachmentsLogicAppName
  params:{
    location:location
     emailAttachmentsLogicAppName:emailAttachmentsLogicAppName
     connectionsAzureblobExternalid:webConnectionsAzureBlob.outputs.Id
     connectionsOffice365Externalid:webConnectionsOffice365.outputs.Id
  }
}



module HostingPlan './templates/hosting-plan.bicep' ={
  name: hostingPlanName
  params: { 
    location:location
    hostingPlanName:hostingPlanName
    isReserved:isReserved
  }
  dependsOn: [
    storageAccountResource
  ]
}

module FunctionAppIntelligentDocument './templates/function-app.bicep'={
  name: functionAppName
  params: {
    functionAppName:functionAppName
    hostingPlanId:HostingPlan.outputs.hostingPlanId
    storageAccountName:storageAccountResource.name
    location:location
    functionWorkerRuntime:functionWorkerRuntime
    netFrameworkVersion:'8.0'
    linuxFxVersion:'DOTNET-ISOLATED|8.0'
    kind:functionAppkind
    applicationInsightsInstrumentationKey:''
    cognitiveServicesAccountName:AIServiceFormRecognizerName
   }
   dependsOn: [
    HostingPlan
    storageAccountResource
  ]
}

