@description('Location for logic app resource.')
param location string 
param emailAttachmentsLogicAppName string 
param subscriptionId string = subscription().subscriptionId
param connectionsAzureblobExternalid string 
param connectionsOffice365Externalid string 

resource workflowsEmailAttachmentsIntoBlobStorageLogicAppResource 'Microsoft.Logic/workflows@2019-05-01' = {
  name: emailAttachmentsLogicAppName
  location: location
  properties: {
    state: 'Enabled'
    definition: {
      '$schema': 'https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#'
      contentVersion: '1.0.0.0'
      parameters: {
        '$connections': {
          defaultValue: {}
          type: 'Object'
        }
      }
      triggers: {
        'When_a_new_email_arrives_(V3)': {
          splitOn: '@triggerBody()?[\'value\']'
          type: 'ApiConnectionNotification'
          inputs: {
            fetch: {
              method: 'get'
              pathTemplate: {
                template: '/v3/Mail/OnNewEmail'
              }
              queries: {
                fetchOnlyWithAttachment: true
                folderPath: 'Inbox'
                importance: 'Any'
                includeAttachments: true                
              }
            }
            host: {
              connection: {
                name: '@parameters(\'$connections\')[\'office365\'][\'connectionId\']'
              }
            }
            subscribe: {
              body: {
                NotificationUrl: '@{listCallbackUrl()}'
              }
              method: 'post'
              pathTemplate: {
                template: '/GraphMailSubscriptionPoke/$subscriptions'
              }
              queries: {
                fetchOnlyWithAttachment: true
                folderPath: 'Inbox'
                importance: 'Any'
              }
            }
          }
        }
      }
      actions: {
        For_each: {
          foreach: '@triggerBody()?[\'attachments\']'
          actions: {
            'Create_blob_(V2)': {
              runAfter: {}
              type: 'ApiConnection'
              inputs: {
                body: '@base64ToBinary(items(\'For_each\')?[\'contentBytes\'])'
                headers: {
                  ReadFileMetadataFromServer: true
                }
                host: {
                  connection: {
                    name: '@parameters(\'$connections\')[\'azureblob\'][\'connectionId\']'
                  }
                }
                method: 'post'
                path: '/v2/datasets/@{encodeURIComponent(encodeURIComponent(\'AccountNameFromSettings\'))}/files'
                queries: {
                  folderPath: '/raw-customer-invoice-email-attachments/@{variables(\'year\')}@{variables(\'Month\')}'
                  name: '@items(\'For_each\')?[\'name\']'
                  queryParametersSingleEncoded: true
                }
              }
              runtimeConfiguration: {
                contentTransfer: {
                  transferMode: 'Chunked'
                }
              }
            }
          }
          runAfter: {
            Set_Month: [
              'Succeeded'
            ]
          }
          type: 'Foreach'
        }
        Set_Month: {
          runAfter: {
            Set_Year: [
              'Succeeded'
            ]
          }
          type: 'InitializeVariable'
          inputs: {
            variables: [
              {
                name: 'Month'
                type: 'string'
                value: '@{variables(\'yearMonthArray\')[1]}'
              }
            ]
          }
        }
        Set_Year: {
          runAfter: {
            yearMonthArray: [
              'Succeeded'
            ]
          }
          type: 'InitializeVariable'
          inputs: {
            variables: [
              {
                name: 'year'
                type: 'string'
                value: '@{variables(\'yearMonthArray\')[0]}'
              }
            ]
          }
        }
        yearMonthArray: {
          runAfter: {}
          type: 'InitializeVariable'
          inputs: {
            variables: [
              {
                name: 'yearMonthArray'
                type: 'array'
                value: [
                  '@substring(utcNow(), 0, 4)'
                  '@substring(utcNow(), 5, 2)'
                ]
              }
            ]
          }
        }
      }
      outputs: {}
    }
    parameters: {
      '$connections': {
        value: {
          azureblob: {
            connectionId: connectionsAzureblobExternalid
            connectionName: 'azureblob'
            id: '/subscriptions/${subscriptionId}/providers/Microsoft.Web/locations/${location}/managedApis/azureblob'
          }
          office365: {
            connectionId: connectionsOffice365Externalid
            connectionName: 'office365'
            id: '/subscriptions/${subscriptionId}/providers/Microsoft.Web/locations/${location}/managedApis/office365'
          }
        }
      }
    }
  }
}
