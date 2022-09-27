var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseBlazorFrameworkFiles();

app.UseDefaultFiles()
   .UseStaticFiles();

app.Run();