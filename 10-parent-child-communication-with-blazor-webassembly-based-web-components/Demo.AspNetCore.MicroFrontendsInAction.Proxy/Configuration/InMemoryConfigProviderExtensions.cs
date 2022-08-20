using Yarp.ReverseProxy.Configuration;
using Demo.AspNetCore.MicroFrontendsInAction.Proxy.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class InMemoryConfigProviderExtensions
    {
        public static IReverseProxyBuilder LoadFromMemory(this IReverseProxyBuilder builder, IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            builder.Services.AddSingleton<IProxyConfigProvider>(new InMemoryConfigProvider(routes, clusters));

            return builder;
        }
    }
}
