using EsiNet.Caching;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Caching
{
    internal class EsiNoOpVaryHeaderStore : IVaryHeaderStore
    {
        private IReadOnlyCollection<string> _headerNames = new List<string>().AsReadOnly();

        public void Set(Uri uri, IReadOnlyCollection<string> headerNames)
        { }

        public bool TryGet(Uri uri, out IReadOnlyCollection<string> headerNames)
        {
            headerNames = _headerNames;

            return false;
        }
    }
}
