targetScope = 'resourceGroup'

param location string = resourceGroup().location
param managedIdentityName string
param containerRegistryName string
param containerAppsEnvironmentName string
param containerRegistrySku string = 'Basic'

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' = {
  name: containerRegistryName
  location: location
  sku: {
    name: containerRegistrySku
  }
  properties: { }
}

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' = {
  name: managedIdentityName
  location: location
}

resource acrPullRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '7f951dda-4ed3-4680-a7ca-43fe172d538d' // AcrPull
  scope: subscription()
}

resource acrPullRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: containerRegistry
  name: guid(containerRegistry.id, managedIdentity.id, acrPullRoleDefinition.id)
  properties: {
    roleDefinitionId: acrPullRoleDefinition.id
    principalId: managedIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2022-06-01-preview' = {
  name: containerAppsEnvironmentName
  location: location
  sku: {
    name: 'Consumption'
  }
  properties: { }
}
