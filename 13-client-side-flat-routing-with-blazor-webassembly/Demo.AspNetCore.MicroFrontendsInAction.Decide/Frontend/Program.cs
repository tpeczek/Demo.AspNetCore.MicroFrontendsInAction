using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Demo.AspNetCore.MicroFrontendsInAction.Decide.Frontend.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.RegisterAsCustomElement<ProductFragment>("decide-product");

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddHttpClient("Demo.AspNetCore.MicroFrontendsInAction.Decide.Service", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Demo.AspNetCore.MicroFrontendsInAction.Decide.Service"));

await builder.Build().RunAsync();