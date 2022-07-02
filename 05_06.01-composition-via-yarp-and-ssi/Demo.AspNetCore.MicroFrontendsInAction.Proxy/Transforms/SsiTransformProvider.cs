using System.Text;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms
{
    internal class SsiTransformProvider : ITransformProvider
    {
        private const string SSI_METADATA_FLAG = "SSI";
        private const string SSI_METADATA_FLAG_ON = "ON";

        public static IReadOnlyDictionary<string, string> SsiEnabledMetadata { get; } = new Dictionary<string, string>() { { SSI_METADATA_FLAG, SSI_METADATA_FLAG_ON } };

        public void ValidateRoute(TransformRouteValidationContext context)
        { }

        public void ValidateCluster(TransformClusterValidationContext context)
        { }

        public void Apply(TransformBuilderContext transformBuildContext)
        {
            if ((transformBuildContext.Route.Metadata is not null) && transformBuildContext.Route.Metadata.ContainsKey(SSI_METADATA_FLAG) && (transformBuildContext.Route.Metadata[SSI_METADATA_FLAG] == SSI_METADATA_FLAG_ON))
            {
                transformBuildContext.AddResponseTransform(TransformResponse);
            }
        }

        private async ValueTask TransformResponse(ResponseTransformContext responseContext)
        {
            if (responseContext.ProxyResponse is null)
            {
                return;
            }

            Stream proxyResponseContentStream = await responseContext.ProxyResponse.Content.ReadAsStreamAsync();

            using StreamReader proxyResponseContentReader = new StreamReader(proxyResponseContentStream);

            string proxyResponseContent = await proxyResponseContentReader.ReadToEndAsync();

            if (!String.IsNullOrEmpty(proxyResponseContent))
            {
                IList<ISsiDirective> ssiDirectives = SsiParser.ParseDirectives(proxyResponseContent);

                responseContext.SuppressResponseBody = true;

                byte[] proxyResponseContentBytes = Encoding.UTF8.GetBytes(proxyResponseContent);
                responseContext.HttpContext.Response.ContentLength = proxyResponseContentBytes.Length;
                await responseContext.HttpContext.Response.Body.WriteAsync(proxyResponseContentBytes);
            }
        }
    }
}
