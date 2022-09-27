using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Demo.AspNetCore.MicroFrontendsInAction.Inspire.Frontend.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.RegisterAsCustomElement<HomeFragment>("inspire-home");

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddHttpClient("Demo.AspNetCore.MicroFrontendsInAction.Inspire.Service", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Demo.AspNetCore.MicroFrontendsInAction.Inspire.Service"));

await builder.Build().RunAsync();
