targetScope = 'resourceGroup'

param location string = resourceGroup().location
param managedIdentityName string
param containerRegistryName string
param containerAppsEnvironmentName string
param microFrontendsContainerAppsDetails array

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' existing = {
  name: managedIdentityName
}

resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2022-06-01-preview' existing = {
  name: containerAppsEnvironmentName
}

module microFrontendsContainerApps 'micro-frontends-in-action-container-app.bicep' = [for microFrontendsContainerAppDetails in microFrontendsContainerAppsDetails: {
  name: '${microFrontendsContainerAppDetails.name}-deployment'
  scope: resourceGroup()
  params: {
    location: location
    containerAppName: microFrontendsContainerAppDetails.name
    containerAppImageName: microFrontendsContainerAppDetails.imageName
    containerAppImageTag: microFrontendsContainerAppDetails.imageTag
    containerAppPort: microFrontendsContainerAppDetails.port
    managedIdentityId: managedIdentity.id
    containerRegistryName: containerRegistryName
    containerAppsEnvironmentId: containerAppsEnvironment.id
  }
}]
