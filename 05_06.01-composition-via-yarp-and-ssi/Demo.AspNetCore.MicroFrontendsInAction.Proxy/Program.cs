using Yarp.ReverseProxy.Configuration;
using Demo.AspNetCore.MicroFrontendsInAction.Proxy;
using Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

var routes = new[]
{
    // Per service prefixes
    new RouteConfig { RouteId = Constants.ROOT_ROUTE_ID, ClusterId = Constants.DECIDE_CLUSTER_ID, Match = new RouteMatch { Path = "/" }, Metadata = SsiTransformProvider.SsiEnabledMetadata },
    // Per service prefixes
    new RouteConfig { RouteId = Constants.DECIDE_ROUTE_ID, ClusterId = Constants.DECIDE_CLUSTER_ID, Match = new RouteMatch { Path = "/decide/{**catch-all}" }, Metadata = SsiTransformProvider.SsiEnabledMetadata },
    new RouteConfig { RouteId = Constants.INSPIRE_ROUTE_IDE, ClusterId = Constants.INSPIRE_CLUSTER_ID, Match = new RouteMatch { Path = "/inspire/{**catch-all}" } },
    // Per page prefixes
    new RouteConfig { RouteId = Constants.PRODUCT_ROUTE_ID, ClusterId = Constants.DECIDE_CLUSTER_ID, Match = new RouteMatch { Path = "/product/{**catch-all}" }, Metadata = SsiTransformProvider.SsiEnabledMetadata },
    new RouteConfig { RouteId = Constants.RECOMMENDATIONS_ROUTE_ID, ClusterId = Constants.INSPIRE_CLUSTER_ID, Match = new RouteMatch { Path = "/recommendations/{**catch-all}" } }
};

var clusters = new[]
{
    new ClusterConfig() { ClusterId = Constants.DECIDE_CLUSTER_ID, Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) { { Constants.DECIDE_SERVICE_URL, new DestinationConfig() { Address = builder.Configuration[Constants.DECIDE_SERVICE_URL] } } } },
    new ClusterConfig() { ClusterId = Constants.INSPIRE_CLUSTER_ID, Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase) { { Constants.INSPIRE_SERVICE_URL, new DestinationConfig() { Address = builder.Configuration[Constants.INSPIRE_SERVICE_URL] } } } }
};

builder.Services.AddReverseProxy()
    .LoadFromMemory(routes, clusters)
    .AddTransforms<SsiTransformProvider>();

var app = builder.Build();

app.MapReverseProxy();

app.Run();
