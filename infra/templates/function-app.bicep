@description('The name of the function app.')
param functionAppName string 

@description('The ID of the hosting plan for the function app.')
param hostingPlanId string 

@description('The name of the storage account for the function app.')
param storageAccountName string 

@description('The location for all resources. Defaults to the location of the resource group.')
param location string = resourceGroup().location

@description('The language worker runtime to load in the function app. Allowed values are "dotnet", "node", "python", "java", and "dotnet-isolated". Defaults to "dotnet".')
@allowed([
  'dotnet'
  'node'
  'python'
  'java'
  'dotnet-isolated'
])
param functionWorkerRuntime string = 'dotnet'

@description('The instrumentation key for Application Insights in the function app.')
@secure()
param applicationInsightsInstrumentationKey string = ''

@description('The kind of the function app. Allowed values are "functionapp,linux" and "functionapp".')
@allowed([
  'functionapp,linux'
  'functionapp'
])
param kind string

param netFrameworkVersion string = '8.0'
@description('Required for Linux app to represent runtime stack in the format of \'runtime|runtimeVersion\'. For example: \'python|3.10\'')
param linuxFxVersion string = 'DOTNET-ISOLATED|8.0'

param cognitiveServicesAccountName string = ''

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' existing ={
  name: storageAccountName
}

resource cognitiveService 'Microsoft.CognitiveServices/accounts@2022-03-01' existing = { 
name: cognitiveServicesAccountName
}

resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: functionAppName
  location: location
  kind: kind
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlanId    
    siteConfig: {
      linuxFxVersion:linuxFxVersion
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(functionAppName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsightsInstrumentationKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionWorkerRuntime
        }
        {
          name: 'FormRecognizerKey1'
          value: cognitiveService.listKeys().key1
        }
        {
          name: 'FormRecognizerEndpoint'
          value: cognitiveService.properties.endpoint
        }
      ]     
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
      netFrameworkVersion:netFrameworkVersion
    }
    httpsOnly: true
  }
  dependsOn: [
    storageAccount
  ]
}

output name string = functionApp.name
output id string = functionApp.id
output principalId string = functionApp.identity.principalId

