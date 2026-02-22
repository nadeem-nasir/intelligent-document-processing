@description('Location for connections resource.')
param location string 
@description('web connections name')
param webConnectionsName string
@description('Resource reference id')
param webConnectionsExternalid string

resource webConnections 'Microsoft.Web/connections@2016-06-01' = {
  name: webConnectionsName
  location: location
  properties: {
    api: {
      id: webConnectionsExternalid   
    }
    customParameterValues: {}
    displayName: webConnectionsName
  }
}

output apiId string = webConnections.properties.api.id
output Id string = webConnections.id
