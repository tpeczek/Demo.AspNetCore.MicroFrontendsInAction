using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Configuration
{
    internal class InMemoryConfigProvider : IProxyConfigProvider
    {
        private class InMemoryConfig : IProxyConfig
        {
            public IReadOnlyList<RouteConfig> Routes { get; }

            public IReadOnlyList<ClusterConfig> Clusters { get; }

            public IChangeToken ChangeToken { get; }

            public InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
            {
                Routes = routes;
                Clusters = clusters;
                ChangeToken = new CancellationChangeToken(CancellationToken.None);
            }
        }

        private InMemoryConfig _config;

        public InMemoryConfigProvider(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            _config = new InMemoryConfig(routes, clusters);
        }

        public IProxyConfig GetConfig() => _config;
    }
}
