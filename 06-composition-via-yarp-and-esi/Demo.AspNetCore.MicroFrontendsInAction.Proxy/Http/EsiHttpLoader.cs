using EsiNet;
using EsiNet.Http;
using EsiNet.Logging;
using EsiNet.Pipeline;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Http
{
    internal class EsiHttpLoader : IHttpLoader
    {
        private readonly EsiHttpClientFactory _httpClientFactory;
        private readonly HttpRequestMessageFactory _httpRequestMessageFactory;
        private readonly IReadOnlyCollection<IHttpLoaderPipeline> _pipelines;
        private readonly Log _log;

        public EsiHttpLoader(EsiHttpClientFactory httpClientFactory, HttpRequestMessageFactory httpRequestMessageFactory, IEnumerable<IHttpLoaderPipeline> pipelines, Log log)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpRequestMessageFactory = httpRequestMessageFactory ?? throw new ArgumentNullException(nameof(httpRequestMessageFactory));
            _pipelines = pipelines?.Reverse().ToArray() ?? throw new ArgumentNullException(nameof(pipelines));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<HttpResponseMessage> Get(Uri uri, EsiExecutionContext executionContext)
        {
            ArgumentNullException.ThrowIfNull(uri, nameof(uri));

            try
            {
                var response = await Execute(uri, executionContext);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (Exception ex)
            {
                _log.Error(() => $"Error when loading '{uri}'.", ex);
                throw;
            }
        }

        private Task<HttpResponseMessage> Execute(Uri uri, EsiExecutionContext executionContext)
        {
            Task<HttpResponseMessage> Send(Uri u, EsiExecutionContext ec) => ExecuteRequest(uri, executionContext);

            return _pipelines.Aggregate((HttpLoadDelegate)Send, (next, pipeline) => async (u, ec) => await pipeline.Handle(u, ec, next))(uri, executionContext);
        }

        private Task<HttpResponseMessage> ExecuteRequest(Uri uri, EsiExecutionContext executionContext)
        {
            var request = _httpRequestMessageFactory(uri, executionContext);

            var httpClient = _httpClientFactory(uri);
            return httpClient.SendAsync(request, CancellationToken.None);
        }
    }
}
