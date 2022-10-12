using Yarp.ReverseProxy.Model;
using Microsoft.AspNetCore.Routing.Template;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms.Ssi.Processing
{
    internal class SsiIncludeDirectiveProcessor : ISsiDirectiveProcessor
    {
        private const string DIRECTIVE = "include";
        private const string VIRTUAL_PARAMETER = "virtual";

        private static readonly RouteValueDictionary _emptyRouteValueDictionary = new RouteValueDictionary();

        public static string Directive { get; } = DIRECTIVE;

        public async Task<string> Process(ISsiDirective directive, HttpContext context)
        {
            if ((directive.Directive != Directive) && (directive.Parameters.Count != 1) && !directive.Parameters.ContainsKey(VIRTUAL_PARAMETER))
            {
                return String.Empty;
            }

            Endpoint? virtualEndpoint = GetVirtualEndpoint(directive, context);
            if (virtualEndpoint is null)
            {
                return String.Empty;
            }

            ClusterModel? cluster = GetProxyCluster(virtualEndpoint);
            if (cluster is null)
            {
                return String.Empty;
            }

            if (cluster.Config.Destinations?.Any() ?? false)
            {
                string virtualUri = cluster.Config.Destinations.FirstOrDefault().Value.Address + GetVirtualPath(directive.Parameters[VIRTUAL_PARAMETER]);

                HttpResponseMessage response = await cluster.HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, virtualUri), CancellationToken.None);

                return await response.Content.ReadAsStringAsync();
            }

            return String.Empty;
        }

        private static Endpoint? GetVirtualEndpoint(ISsiDirective directive, HttpContext context)
        {
            var endpointDataSource = context.RequestServices.GetService<EndpointDataSource>();

            if (endpointDataSource is not null)
            {
                var virtualPath = new PathString(directive.Parameters[VIRTUAL_PARAMETER]);
                foreach (Endpoint possibleVirtualEndpoint in endpointDataSource.Endpoints)
                {
                    var routeEndpoint = possibleVirtualEndpoint as RouteEndpoint;
                    if (routeEndpoint is not null)
                    {
                        var routeTemplateMatcher = new TemplateMatcher(new RouteTemplate(routeEndpoint.RoutePattern), _emptyRouteValueDictionary);
                        if (routeTemplateMatcher.TryMatch(virtualPath, _emptyRouteValueDictionary))
                        {
                            return possibleVirtualEndpoint;
                        }
                    }
                }
            }

            return null;
        }

        private static ClusterModel? GetProxyCluster(Endpoint virtualEndpoint)
        {
            foreach (var endpointMetadata in virtualEndpoint.Metadata)
            {
                var proxyRoute = endpointMetadata as RouteModel;
                if (proxyRoute is not null)
                {
                    return proxyRoute.Cluster?.Model;
                }
            }

            return null;
        }

        private static string GetVirtualPath(string virtualParameter)
        {
            if (virtualParameter.StartsWith(Constants.CHECKOUT_ROUTE_PREFIX))
            {
                return virtualParameter.Substring(Constants.CHECKOUT_ROUTE_PREFIX.Length);
            }

            if (virtualParameter.StartsWith(Constants.DECIDE_ROUTE_PREFIX))
            {
                return virtualParameter.Substring(Constants.DECIDE_ROUTE_PREFIX.Length);
            }

            if (virtualParameter.StartsWith(Constants.INSPIRE_ROUTE_PREFIX))
            {
                return virtualParameter.Substring(Constants.INSPIRE_ROUTE_PREFIX.Length);
            }

            return virtualParameter;
        }
    }
}
