var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(name: "recommendations-fragments", pattern: "inspire/fragment/recommendations/{action}", defaults: new { controller = "RecommendationsFragments", action = "Article" });
app.MapControllerRoute(name: "recommendations", pattern: "recommendations/{action}", defaults: new { controller = "Recommendations" });

app.Run();
