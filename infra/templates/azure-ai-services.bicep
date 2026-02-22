
@description('That name is the name of our application. It has to be unique.Type a name followed by your resource group name. (<name>-<resourceGroupName>)')
param AIServiceName string 

@description('Location for resources.')
param location string 

@description('The kind field in the Bicep file defines the type of resource.')
@allowed([
'ComputerVision'
'CustomVision.Prediction'
'CustomVision.Training'
'Face'
'FormRecognizer'
'SpeechServices'
'OpenAI'
'Personalizer'
'ContentModerator'
'AnomalyDetector'
'TextTranslation'
'TextAnalytics'
'QnAMaker'
'LUIS'
'SpeechServices'
'CognitiveServices'
])
param kind string

@allowed([
  'S0'
  'F0'
  'S1'
])
param sku string = 'S0'

resource cognitiveService 'Microsoft.CognitiveServices/accounts@2022-03-01' = {
  name: AIServiceName
  location: location
  sku: {
    name: sku
  }
  kind: kind
  properties: {
    apiProperties: {
      statisticsEnabled: false
    }
  }
}

output cognitiveServiceEndpoint string = cognitiveService.properties.endpoint
output id string = cognitiveService.id

