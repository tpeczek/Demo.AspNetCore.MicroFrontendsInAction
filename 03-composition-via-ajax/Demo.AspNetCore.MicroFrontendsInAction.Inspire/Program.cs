using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseRewriter(new RewriteOptions()
    .AddRedirect("recommendations/(.*).html", "recommendations/$1")
    .AddRewrite("recommendations/(.*)", "recommendations/$1.html", skipRemainingRules: true)
    .AddRedirect("fragment/recommendations/(.*).html", "fragment/recommendations/$1")
    .AddRewrite("fragment/recommendations/(.*)", "fragment/recommendations/$1.html", skipRemainingRules: true));

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = (ctx) =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:3001");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Methods", "GET");
    }
});

app.Run();
