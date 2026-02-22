@description('Storage account names must be between 3 and 24 characters in length and may contain numbers and lowercase letters only. Your storage account name must be unique within Azure')
@minLength(3)
@maxLength(22)
param storageAccountName string

@description('Object Id of the user or User ID in AD')
param principalId string

@allowed([
'Device'
'ForeignGroup'
'Group'
'ServicePrincipal'
'User'
''
])
param principalType string = ''

@description('role definition Id')
param roleDefinitionId string


resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' existing = {
  name: storageAccountName
}

resource roleAuthorization 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('storage-rbac', storageAccount.id, resourceGroup().id, principalId, roleDefinitionId)
  scope: storageAccount
  properties: {
      principalId: principalId
      roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions',roleDefinitionId)
      principalType: empty(principalType) ? null : principalType
  }
}



