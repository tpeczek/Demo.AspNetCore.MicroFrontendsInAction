var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(name: "products", pattern: "product/{action}", defaults: new { controller = "Products" });

app.Run();
