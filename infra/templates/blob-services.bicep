
@description('storage containers names')
param  storagecontainersNames array 

@description('Storage account names must be between 3 and 24 characters in length and may contain numbers and lowercase letters only. Your storage account name must be unique within Azure')
@minLength(3)
@maxLength(22)
param storageAccountName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' existing = {
  name: storageAccountName
}

resource blobServices 'Microsoft.Storage/storageAccounts/blobServices@2022-09-01' existing = { 
name:'default'
parent:storageAccount

}
resource containersBlobServices 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-09-01' = [for containerName in range(0, length(storagecontainersNames)): {
  name: storagecontainersNames[containerName]
  parent:blobServices
  properties: {
    metadata: {}
    publicAccess: 'None'
  }
}]
