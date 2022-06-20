using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRewriter(new RewriteOptions()
    .AddRedirect("recommendations/(.*).html", "recommendations/$1")
    .AddRewrite("recommendations/(.*)", "recommendations/$1.html", skipRemainingRules: true));

app.UseStaticFiles();

app.Run();
