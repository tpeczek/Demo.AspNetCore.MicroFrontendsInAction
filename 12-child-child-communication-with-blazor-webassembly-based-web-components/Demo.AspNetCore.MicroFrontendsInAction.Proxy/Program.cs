using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Configuration;
using Demo.AspNetCore.MicroFrontendsInAction.Proxy;

var builder = WebApplication.CreateBuilder(args);

var routes = new[]
{
    // Per service prefixes
    new RouteConfig { RouteId = Constants.ROOT_ROUTE_ID, ClusterId = Constants.DECIDE_CLUSTER_ID, Match = new RouteMatch { Path = "/" } },
    // Per service prefixes
    (new RouteConfig { RouteId = Constants.DECIDE_ROUTE_ID, ClusterId = Constants.DECIDE_CLUSTER_ID, Match = new RouteMatch { Path = Constants.DECIDE_ROUTE_PREFIX + "/{**catch-all}" } }).WithTransformPathRemovePrefix(Constants.DECIDE_ROUTE_PREFIX),
    (new RouteConfig { RouteId = Constants.INSPIRE_ROUTE_ID, ClusterId = Constants.INSPIRE_CLUSTER_ID, Match = new RouteMatch { Path = Constants.INSPIRE_ROUTE_PREFIX + "/{**catch-all}" } }).WithTransformPathRemovePrefix(Constants.INSPIRE_ROUTE_PREFIX),
    (new RouteConfig { RouteId = Constants.CHECKOUT_ROUTE_ID, ClusterId = Constants.CHECKOUT_CLUSTER_ID, Match = new RouteMatch { Path = Constants.CHECKOUT_ROUTE_PREFIX + "/{**catch-all}" } }).WithTransformPathRemovePrefix(Constants.CHECKOUT_ROUTE_PREFIX),
    // Per page prefixes
    new RouteConfig { RouteId = Constants.PRODUCT_ROUTE_ID, ClusterId = Constants.DECIDE_CLUSTER_ID, Match = new RouteMatch { Path = "/product/{**catch-all}" } },
    new RouteConfig { RouteId = Constants.RECOMMENDATIONS_ROUTE_ID, ClusterId = Constants.INSPIRE_CLUSTER_ID, Match = new RouteMatch { Path = "/recommendations/{**catch-all}" } },
    // Resources prefixes
    new RouteConfig { RouteId = "blazor-broadcast-channel-route", ClusterId = Constants.CHECKOUT_CLUSTER_ID, Match = new RouteMatch { Path = "/_content/Blazor.BroadcastChannel/{**catch-all}" } }
};

var clusters = new[]
{
    new ClusterConfig() { ClusterId = Constants.DECIDE_CLUSTER_ID, Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) { { Constants.DECIDE_SERVICE_URL, new DestinationConfig() { Address = builder.Configuration[Constants.DECIDE_SERVICE_URL] } } } },
    new ClusterConfig() { ClusterId = Constants.INSPIRE_CLUSTER_ID, Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) { { Constants.INSPIRE_SERVICE_URL, new DestinationConfig() { Address = builder.Configuration[Constants.INSPIRE_SERVICE_URL] } } } },
    new ClusterConfig() { ClusterId = Constants.CHECKOUT_CLUSTER_ID, Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) { { Constants.CHECKOUT_SERVICE_URL, new DestinationConfig() { Address = builder.Configuration[Constants.CHECKOUT_SERVICE_URL] } } } }
};

builder.Services.AddReverseProxy()
    .LoadFromMemory(routes, clusters);

var app = builder.Build();

app.MapReverseProxy();

app.Run();
