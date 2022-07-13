namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms.Ssi
{
    internal interface ISsiDirective
    {
        string Directive { get; }

        public IReadOnlyDictionary<string, string> Parameters { get; }

        public int Index { get; }

        public int Length { get; }
    }
}
