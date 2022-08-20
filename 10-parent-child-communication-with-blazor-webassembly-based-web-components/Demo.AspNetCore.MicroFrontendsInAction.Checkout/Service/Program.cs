using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

Console.WriteLine(builder.Environment.WebRootPath);

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapFallbackToFile("index.html");

app.Run();
