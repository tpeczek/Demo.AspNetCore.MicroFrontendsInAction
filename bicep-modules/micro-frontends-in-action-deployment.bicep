targetScope = 'resourceGroup'

param location string = resourceGroup().location
param managedIdentityName string
param containerRegistryName string
param containerAppsEnvironmentName string
param microFrontendsContainerAppsDetails array
param microFrontendsProxyContainerAppDetails object

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

module proxyContainerApp 'micro-frontends-in-action-container-app.bicep' = {
  name: '${microFrontendsProxyContainerAppDetails.name}-deployment'
  scope: resourceGroup()
  params: {
    location: location
    containerAppName: microFrontendsProxyContainerAppDetails.name
    containerAppImageName: microFrontendsProxyContainerAppDetails.imageName
    containerAppImageTag: microFrontendsProxyContainerAppDetails.imageTag
    containerAppPort: microFrontendsProxyContainerAppDetails.port
    containerAppIngress: 'external'
    containerAppEnvironmentVariables: [for (microFrontendsContainerAppDetails, i) in microFrontendsContainerAppsDetails: {
      name: microFrontendsContainerAppDetails.urlVariableName
      value: 'https://${microFrontendsContainerApps[i].outputs.containerAppFqdn}'
    }]
    managedIdentityId: managedIdentity.id
    containerRegistryName: containerRegistryName
    containerAppsEnvironmentId: containerAppsEnvironment.id
  }
}
