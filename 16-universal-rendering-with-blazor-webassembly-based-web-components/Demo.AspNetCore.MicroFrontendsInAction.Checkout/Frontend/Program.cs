using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Demo.AspNetCore.MicroFrontendsInAction.Checkout.Frontend.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.RegisterAsCustomElement<BuyButton>("checkout-buy");
builder.RootComponents.RegisterAsCustomElement<MiniCart>("checkout-minicart");

builder.Services.AddBroadcastChannel();
builder.Services.AddHttpClient("Demo.AspNetCore.MicroFrontendsInAction.Checkout.Service", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Demo.AspNetCore.MicroFrontendsInAction.Checkout.Service"));

await builder.Build().RunAsync();
