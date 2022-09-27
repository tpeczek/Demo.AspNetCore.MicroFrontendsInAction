using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Configuration;
using Demo.AspNetCore.MicroFrontendsInAction.Proxy;

var builder = WebApplication.CreateBuilder(args);

var routes = new[]
{
    // Per service prefixes
    (new RouteConfig { RouteId = Constants.DECIDE_SERVICE_ROUTE_ID, ClusterId = Constants.DECIDE_SERVICE_CLUSTER_ID, Match = new RouteMatch { Path = Constants.DECIDE_SERVICE_ROUTE_PREFIX + "/{**catch-all}" } }).WithTransformPathRemovePrefix(Constants.DECIDE_SERVICE_ROUTE_PREFIX),
    (new RouteConfig { RouteId = Constants.INSPIRE_SERVICE_ROUTE_ID, ClusterId = Constants.INSPIRE_SERVICE_CLUSTER_ID, Match = new RouteMatch { Path = Constants.INSPIRE_SERVICE_ROUTE_PREFIX + "/{**catch-all}" } }).WithTransformPathRemovePrefix(Constants.INSPIRE_SERVICE_ROUTE_PREFIX),
    (new RouteConfig { RouteId = Constants.CHECKOUT_SERVICE_ROUTE_ID, ClusterId = Constants.CHECKOUT_SERVICE_CLUSTER_ID, Match = new RouteMatch { Path = Constants.CHECKOUT_SERVICE_ROUTE_PREFIX + "/{**catch-all}" } }).WithTransformPathRemovePrefix(Constants.CHECKOUT_SERVICE_ROUTE_PREFIX),
    // Application shell prefix
    new RouteConfig { RouteId = Constants.APPLICATION_SHELL_ROUTE_ID, ClusterId = Constants.APPLICATION_SHELL_CLUSTER_ID, Match = new RouteMatch { Path = "/{**catch-all}" } },
};

var clusters = new[]
{
    new ClusterConfig() { ClusterId = Constants.APPLICATION_SHELL_CLUSTER_ID, Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) { { Constants.APPLICATION_SHELL_URL, new DestinationConfig() { Address = builder.Configuration[Constants.APPLICATION_SHELL_URL] } } } },
    new ClusterConfig() { ClusterId = Constants.DECIDE_SERVICE_CLUSTER_ID, Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) { { Constants.DECIDE_SERVICE_URL, new DestinationConfig() { Address = builder.Configuration[Constants.DECIDE_SERVICE_URL] } } } },
    new ClusterConfig() { ClusterId = Constants.INSPIRE_SERVICE_CLUSTER_ID, Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) { { Constants.INSPIRE_SERVICE_URL, new DestinationConfig() { Address = builder.Configuration[Constants.INSPIRE_SERVICE_URL] } } } },
    new ClusterConfig() { ClusterId = Constants.CHECKOUT_SERVICE_CLUSTER_ID, Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) { { Constants.CHECKOUT_SERVICE_URL, new DestinationConfig() { Address = builder.Configuration[Constants.CHECKOUT_SERVICE_URL] } } } }
};

builder.Services.AddReverseProxy()
    .LoadFromMemory(routes, clusters);

var app = builder.Build();

app.MapReverseProxy();

app.Run();