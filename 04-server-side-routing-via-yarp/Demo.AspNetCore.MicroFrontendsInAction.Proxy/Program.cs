using Demo.AspNetCore.MicroFrontendsInAction.Proxy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpForwarder();

var app = builder.Build();

string decideServiceUrl = app.Configuration["DECIDE_SERVICE_URL"];
string inspireServiceUrl = app.Configuration["INSPIRE_SERVICE_URL"];

// Per service prefixes
app.MapForwarder("/", decideServiceUrl);

// Per service prefixes
app.MapForwarder("/decide/{**catch-all}", decideServiceUrl);
app.MapForwarder("/inspire/{**catch-all}", inspireServiceUrl);

// Per page prefixes
app.MapForwarder("/product/{**catch-all}", decideServiceUrl);
app.MapForwarder("/recommendations/{**catch-all}", inspireServiceUrl);

app.Run();
