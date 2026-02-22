@description('Specify the type of storage account. Valid options: Standard_LRS')
@allowed([
  'Standard_LRS'
])
param storageAccountType string 


@description('Specify a unique name for the storage account. The name must be between 3 and 24 characters in length and can only contain numbers and lowercase letters.')
@minLength(3)
@maxLength(24)
param storageAccountName string


@description('Specify the location where the storage account will be created.')
param location string 

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountType
  }
  kind: 'Storage'
  properties: {
    supportsHttpsTrafficOnly: true
    defaultToOAuthAuthentication: true
  }
}


output storageAccountId string = storageAccount.id
output storageAccountName string = storageAccount.name


