var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(name: "products", pattern: "product/{id}", defaults: new { controller = "Products", action = "Product" });
app.MapControllerRoute(name: "default", pattern: "/", defaults: new { controller = "Products", action = "Product", id = "porsche" });

app.Run();
