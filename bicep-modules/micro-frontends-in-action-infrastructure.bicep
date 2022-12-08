targetScope = 'subscription'

param resourceGroupName string
param resourceGroupLocation string
param managedIdentityName string
param containerRegistryName string
param containerAppsEnvironmentName string

resource resourceGroupReference 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroupName
  location: resourceGroupLocation
}

module resourceGroupModule 'micro-frontends-in-action-resource-group.bicep' = {
  name: 'micro-frontends-in-action-rg'
  scope: resourceGroup(resourceGroupReference.name)
  params: {
    location: resourceGroupReference.location
    managedIdentityName: managedIdentityName
    containerRegistryName: containerRegistryName
    containerAppsEnvironmentName: containerAppsEnvironmentName
  }
}
