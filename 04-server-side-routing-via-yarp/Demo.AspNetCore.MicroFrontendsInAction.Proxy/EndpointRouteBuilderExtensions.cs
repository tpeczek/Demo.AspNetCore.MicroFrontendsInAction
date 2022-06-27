using System.Net;
using System.Diagnostics;
using Yarp.ReverseProxy.Forwarder;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy
{
    internal static class EndpointRouteBuilderExtensions
    {
        private static HttpMessageInvoker? _httpClient;

        private static HttpMessageInvoker HttpClient
        {
            get
            {
                if (_httpClient is null)
                {
                    _httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
                    {
                        UseProxy = false,
                        AllowAutoRedirect = false,
                        AutomaticDecompression = DecompressionMethods.None,
                        UseCookies = false,
                        ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current)
                    });
                }

                return _httpClient;
            }
        }

        public static void MapForwarder(this IEndpointRouteBuilder endpoints, string pattern, string serviceUrl)
        {
            var forwarder = endpoints.ServiceProvider.GetRequiredService<IHttpForwarder>();
            var requestConfig = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromMilliseconds(200) };

            endpoints.Map(pattern, async httpContext =>
            {
                var error = await forwarder.SendAsync(httpContext, serviceUrl, HttpClient, requestConfig, HttpTransformer.Default);

                if (error != ForwarderError.None)
                {
                    var errorFeature = httpContext.GetForwarderErrorFeature();
                    var exception = errorFeature?.Exception;
                }
            });
        }
    }
}
