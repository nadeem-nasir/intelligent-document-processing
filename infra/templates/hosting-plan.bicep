@description('Location for the hosting plan.')
param location string 

@description('Name for the Azure Function hosting plan.')
param hostingPlanName string 

@description('Set to true for Linux hosting plan.')
param isReserved bool

resource hostingPlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
  properties: {
    reserved: isReserved
  }
}
output hostingPlanId string = hostingPlan.id
output hostingPlanname string = hostingPlan.name
