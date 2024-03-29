targetScope = 'resourceGroup'

param location string = resourceGroup().location
param containerAppName string
param containerAppPort int = 80
@allowed(['internal', 'external'])
param containerAppIngress string = 'internal'
param containerAppImageName string
param containerAppImageTag string
param containerAppEnvironmentVariables array = []
param managedIdentityId string
param containerRegistryName string
param containerAppsEnvironmentId string

resource containerApp 'Microsoft.App/containerApps@2022-06-01-preview' = {
  name: containerAppName
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${managedIdentityId}': {}
    }
  }
  properties:{
    managedEnvironmentId: containerAppsEnvironmentId
    configuration: {
      ingress: {
        targetPort: containerAppPort
        external: containerAppIngress == 'external'
      }
      registries: [
        {
          identity: managedIdentityId
          server: '${containerRegistryName}.azurecr.io'
        }
      ]
    }
    template: {
      containers: [
        {
          name: containerAppImageName
          image: '${containerRegistryName}.azurecr.io/${containerAppImageName}:${containerAppImageTag}'
          env: containerAppEnvironmentVariables
        }
      ]
      scale: {
        minReplicas: 1
      }
    }
  }
}

output containerAppFqdn string = containerApp.properties.configuration.ingress.fqdn
