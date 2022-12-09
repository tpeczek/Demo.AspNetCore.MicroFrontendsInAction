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

module microFrontendsContainerAppsFqdns 'micro-frontends-in-action-container-app.bicep' = [for microFrontendsContainerAppDetails in microFrontendsContainerAppsDetails: {
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

resource microFrontendsProxyContainerApp 'Microsoft.App/containerApps@2022-06-01-preview' = {
  name: microFrontendsProxyContainerAppDetails.name
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${managedIdentity.id}': {}
    }
  }
  properties:{
    managedEnvironmentId: containerAppsEnvironment.id
    configuration: {
      ingress: {
        targetPort: microFrontendsProxyContainerAppDetails.port
        external: true
      }
      registries: [
        {
          identity: managedIdentity.id
          server: containerRegistryName
        }
      ]
    }
    template: {
      containers: [
        {
          image: '${containerRegistryName}.azurecr.io/${microFrontendsProxyContainerAppDetails.imageName}:${microFrontendsProxyContainerAppDetails.containerAppImageTag}'
          name: microFrontendsProxyContainerAppDetails.imageName
        }
      ]
      scale: {
        minReplicas: 1
      }
    }
  }
}
