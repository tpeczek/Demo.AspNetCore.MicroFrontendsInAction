var builder = DistributedApplication.CreateBuilder(args);

var decideApplication = builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Decide>("ca-app-decide");
var inspireApplication = builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Inspire>("ca-app-inspire");

builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Proxy>("ca-app-proxy")
    .WithEnvironment("DECIDE_SERVICE_URL", decideApplication.GetEndpoint("http"))
    .WithEnvironment("INSPIRE_SERVICE_URL", inspireApplication.GetEndpoint("http"));

builder.Build().Run();
