using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRewriter(new RewriteOptions()
    .AddRedirect("product/(.*).html", "product/$1")
    .AddRewrite("product/(.*)", "product/$1.html", skipRemainingRules: true));

app.UseStaticFiles();

app.Run();
