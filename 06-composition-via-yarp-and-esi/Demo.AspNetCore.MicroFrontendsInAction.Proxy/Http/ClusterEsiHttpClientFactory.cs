using EsiNet;
using Microsoft.AspNetCore.Routing.Template;
using System;
using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Http
{
    public delegate HttpMessageInvoker EsiHttpClientFactory(Uri uri);

    internal class ClusterEsiHttpClientFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly RouteValueDictionary _emptyRouteValueDictionary = new RouteValueDictionary();

        public ClusterEsiHttpClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Uri ParseUri(string url)
        {
            Endpoint? virtualEndpoint = GetVirtualEndpoint(url);
            if (virtualEndpoint is null)
            {
                return new Uri(url);
            }

            ClusterState? cluster = GetProxyCluster(virtualEndpoint);
            if (cluster is null)
            {
                return new Uri(url);
            }

            DestinationState? destination = GetDestination(cluster);
            if (cluster is null)
            {
                return new Uri(url);
            }

            return new Uri(new Uri(destination.Model.Config.Address), url);
        }

        public HttpMessageInvoker ProduceHttpClient(Uri uri)
        {
            Endpoint? virtualEndpoint = GetVirtualEndpoint(uri.AbsolutePath);
            if (virtualEndpoint is null)
            {
                return new HttpClient();
            }

            ClusterState? cluster = GetProxyCluster(virtualEndpoint);
            if (cluster?.Model is null)
            {
                return new HttpClient();
            }

            return cluster.Model.HttpClient;
        }

        private Endpoint? GetVirtualEndpoint(PathString path)
        {
            var endpointDataSource = _serviceProvider.GetService<EndpointDataSource>();

            if (endpointDataSource is not null)
            {
                foreach (Endpoint possibleVirtualEndpoint in endpointDataSource.Endpoints)
                {
                    var routeEndpoint = possibleVirtualEndpoint as RouteEndpoint;
                    if (routeEndpoint is not null)
                    {
                        var routeTemplateMatcher = new TemplateMatcher(new RouteTemplate(routeEndpoint.RoutePattern), _emptyRouteValueDictionary);
                        if (routeTemplateMatcher.TryMatch(path, _emptyRouteValueDictionary))
                        {
                            return possibleVirtualEndpoint;
                        }
                    }
                }
            }

            return null;
        }

        private static ClusterState? GetProxyCluster(Endpoint virtualEndpoint)
        {
            foreach (var endpointMetadata in virtualEndpoint.Metadata)
            {
                var proxyRoute = endpointMetadata as RouteModel;
                if (proxyRoute is not null)
                {
                    return proxyRoute.Cluster;
                }
            }

            return null;
        }

        private DestinationState? GetDestination(ClusterState cluster)
        {
            int destinationsCount = cluster.DestinationsState.AvailableDestinations.Count;
            if (destinationsCount == 0)
            {
                return null;
            }
            else if (destinationsCount == 1)
            {
                return cluster.DestinationsState.AvailableDestinations[0];
            }
            else
            {
                var loadBalancingPolicy = _serviceProvider.GetServices<ILoadBalancingPolicy>().FirstOrDefault(loadBalancingPolicy => StringComparer.OrdinalIgnoreCase.Compare(loadBalancingPolicy.Name, cluster.Model.Config.LoadBalancingPolicy ?? LoadBalancingPolicies.PowerOfTwoChoices) == 0);
                return loadBalancingPolicy.PickDestination(null, cluster, cluster.DestinationsState.AvailableDestinations);
            }
        }
    }
}
