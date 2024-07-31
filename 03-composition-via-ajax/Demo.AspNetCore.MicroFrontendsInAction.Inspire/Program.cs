var builder = WebApplication.CreateBuilder(args);

string decideServiceCorsPolicyName = "decide-service-cors-policy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: decideServiceCorsPolicyName, policy =>
    {
        policy.WithOrigins(builder.Configuration["DECIDE_SERVICE_URL"]);
        policy.AllowAnyHeader();
        policy.WithMethods("GET");
    });
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseCors(decideServiceCorsPolicyName);

app.MapControllerRoute(name: "recommendations-fragments", pattern: "fragment/recommendations/{id}", defaults: new { controller = "Recommendations", action = "RecommendationFragment" });
app.MapControllerRoute(name: "recommendations", pattern: "recommendations/{id}", defaults: new { controller = "Recommendations", action = "Recommendation" });
app.MapControllerRoute(name: "default", pattern: "/", defaults: new { controller = "Recommendations", action = "Recommendation", id = "porsche" });

app.Run();
