namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms.Ssi.Processing
{
    internal interface ISsiDirectiveProcessor
    {
        Task<string> Process(ISsiDirective directive, HttpContext context);
    }
}
