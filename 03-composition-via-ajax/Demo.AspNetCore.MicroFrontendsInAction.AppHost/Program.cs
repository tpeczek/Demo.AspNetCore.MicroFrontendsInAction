var builder = DistributedApplication.CreateBuilder(args);

var decideApplication = builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Decide>("ca-app-decide");
var inspireApplication = builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Inspire>("ca-app-inspire");

decideApplication.WithEnvironment("INSPIRE_SERVICE_URL", inspireApplication.GetEndpoint("https"));
inspireApplication.WithEnvironment("DECIDE_SERVICE_URL", decideApplication.GetEndpoint("https"));

Console.WriteLine(decideApplication.GetEndpoint("URL"));

builder.Build().Run();
