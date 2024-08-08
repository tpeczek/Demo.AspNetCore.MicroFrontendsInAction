var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(name: "recommendations-fragments", pattern: "fragment/recommendations/{id}", defaults: new { controller = "Recommendations", action = "RecommendationFragment" });
app.MapControllerRoute(name: "recommendations", pattern: "recommendations/{id}", defaults: new { controller = "Recommendations", action = "Recommendation" });
app.MapControllerRoute(name: "default", pattern: "/", defaults: new { controller = "Recommendations", action = "Recommendation", id = "porsche" });

app.Run();
