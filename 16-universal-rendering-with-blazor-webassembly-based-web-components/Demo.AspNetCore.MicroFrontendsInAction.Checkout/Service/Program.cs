using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBroadcastChannel();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

Console.WriteLine(builder.Environment.WebRootPath);

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(name: "checkout-fragments", pattern: "fragment/buy/{sku}/{edition}", defaults: new { controller = "Fragments", action = "Buy" });

app.Run();
