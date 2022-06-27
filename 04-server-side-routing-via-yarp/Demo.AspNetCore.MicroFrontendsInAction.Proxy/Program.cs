using Demo.AspNetCore.MicroFrontendsInAction.Proxy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpForwarder();

var app = builder.Build();

string decideServiceUrl = app.Configuration["DECIDE_SERVICE_URL"];
string inspireServiceUrl = app.Configuration["INSPIRE_SERVICE_URL"];

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    // Per service prefixes
    endpoints.MapForwarder("/", decideServiceUrl);

    // Per service prefixes
    endpoints.MapForwarder("/decide/{**catch-all}", decideServiceUrl);
    endpoints.MapForwarder("/inspire/{**catch-all}", inspireServiceUrl);

    // Per page prefixes
    endpoints.MapForwarder("/product/{**catch-all}", decideServiceUrl);
    endpoints.MapForwarder("/recommendations/{**catch-all}", inspireServiceUrl);
});

app.Run();
