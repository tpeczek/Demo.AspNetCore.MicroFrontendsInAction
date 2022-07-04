using System.Text;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;
using Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms.Ssi.Parsing;
using Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms.Ssi.Processing;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms.Ssi
{
    internal class SsiTransformProvider : ITransformProvider
    {
        private const string SSI_METADATA_FLAG = "SSI";
        private const string SSI_METADATA_FLAG_ON = "ON";

        public static IReadOnlyDictionary<string, string> SsiEnabledMetadata { get; } = new Dictionary<string, string>() { { SSI_METADATA_FLAG, SSI_METADATA_FLAG_ON } };

        private readonly Dictionary<string, ISsiDirectiveProcessor> _ssiDirectivesProcessors = new Dictionary<string, ISsiDirectiveProcessor>
        {
            { SsiIncludeDirectiveProcessor.Directive, new SsiIncludeDirectiveProcessor() }
        };
        private static readonly Task<String> _notSupportedDirectiveTask = Task.FromResult(String.Empty);

        public void ValidateRoute(TransformRouteValidationContext context)
        { }

        public void ValidateCluster(TransformClusterValidationContext context)
        { }

        public void Apply(TransformBuilderContext transformBuildContext)
        {
            if (transformBuildContext.Route.Metadata is not null && transformBuildContext.Route.Metadata.ContainsKey(SSI_METADATA_FLAG) && transformBuildContext.Route.Metadata[SSI_METADATA_FLAG] == SSI_METADATA_FLAG_ON)
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

            string proxyResponseContent = await responseContext.ProxyResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(proxyResponseContent))
            {
                return;
            }

            responseContext.SuppressResponseBody = true;

            IList<ISsiDirective> directives = SsiParser.ParseDirectives(proxyResponseContent);
            if (directives.Count > 0)
            {
                var directivesProcessingTasks = directives
                    .Select(directive => _ssiDirectivesProcessors.ContainsKey(directive.Directive) ? _ssiDirectivesProcessors[directive.Directive].Process(directive, responseContext.HttpContext) : _notSupportedDirectiveTask)
                    .ToArray();

                await Task.WhenAll(directivesProcessingTasks);

                int proxyResponseContentOffset = 0;
                StringBuilder proxyResponseContentBuilder = new StringBuilder(proxyResponseContent.Length);

                for (int directiveIndex = 0; directiveIndex < directives.Count; directiveIndex++)
                {
                    string directiveProcessingReult = directivesProcessingTasks[directiveIndex].Result;

                    proxyResponseContentBuilder.Append(proxyResponseContent.Substring(proxyResponseContentOffset, directives[directiveIndex].Index - proxyResponseContentOffset));
                    proxyResponseContentBuilder.Append(directiveProcessingReult);

                    proxyResponseContentOffset = directives[directiveIndex].Index + directives[directiveIndex].Length;
                }

                proxyResponseContentBuilder.Append(proxyResponseContent.Substring(proxyResponseContentOffset));

                proxyResponseContent = proxyResponseContentBuilder.ToString();
            }

            byte[] proxyResponseContentBytes = Encoding.UTF8.GetBytes(proxyResponseContent);
            responseContext.HttpContext.Response.ContentLength = proxyResponseContentBytes.Length;
            await responseContext.HttpContext.Response.Body.WriteAsync(proxyResponseContentBytes);
        }
    }
}
