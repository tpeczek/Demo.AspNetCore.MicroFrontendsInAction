using EsiNet;
using EsiNet.Expressions;
using EsiNet.Fragments;
using System.Text;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms.Esi
{
    internal class EsiTransformProvider : ITransformProvider
    {
        private const string ESI_METADATA_FLAG = "ESI";
        private const string ESI_METADATA_FLAG_ON = "ON";

        public static IReadOnlyDictionary<string, string> EsiEnabledMetadata { get; } = new Dictionary<string, string>() { { ESI_METADATA_FLAG, ESI_METADATA_FLAG_ON } };

        public void Apply(TransformBuilderContext context)
        {
            if (context.Route.Metadata is not null && context.Route.Metadata.ContainsKey(ESI_METADATA_FLAG) && context.Route.Metadata[ESI_METADATA_FLAG] == ESI_METADATA_FLAG_ON)
            {
                context.AddResponseTransform(TransformResponse);
            }
        }

        public void ValidateCluster(TransformClusterValidationContext context)
        { }

        public void ValidateRoute(TransformRouteValidationContext context)
        { }

        private async ValueTask TransformResponse(ResponseTransformContext responseContext)
        {
            if (responseContext.ProxyResponse is null)
            {
                return;
            }

            string proxyResponseContent = await responseContext.ProxyResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(proxyResponseContent))
            {
                return;
            }

            responseContext.SuppressResponseBody = true;

            var esiParser = responseContext.HttpContext.RequestServices.GetRequiredService<EsiBodyParser>();
            var esiExecutor = responseContext.HttpContext.RequestServices.GetRequiredService<EsiFragmentExecutor>();

            IEsiFragment esiFragment = esiParser.Parse(proxyResponseContent);

            EsiExecutionContext esiExecutionContext = new EsiExecutionContext(
                responseContext.HttpContext.Request.Headers.ToDictionary(header => header.Key, header => (IReadOnlyCollection<string>)header.Value.ToArray(), StringComparer.OrdinalIgnoreCase),
                new Dictionary<string, IVariableValueResolver>());
            var responseContentParts = await esiExecutor.Execute(esiFragment, esiExecutionContext);

            string responseContent = String.Join(String.Empty, responseContentParts);
            byte[] responseContentBytes = Encoding.UTF8.GetBytes(responseContent);

            responseContext.HttpContext.Response.ContentLength = responseContentBytes.Length;
            await responseContext.HttpContext.Response.Body.WriteAsync(responseContentBytes);
        }
    }
}
