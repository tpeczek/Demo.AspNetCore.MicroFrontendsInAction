var builder = DistributedApplication.CreateBuilder(args);

var decideApplication = builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Decide>("ca-app-decide");
var inspireApplication = builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Inspire>("ca-app-inspire");
var checkoutApplication = builder.AddDockerfile("ca-app-checkout", "..\\Demo.AspNetCore.MicroFrontendsInAction.Checkout")
                                 .WithEndpoint(scheme: "http", targetPort: 3003);

builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Proxy>("ca-app-proxy")
    .WithEnvironment("DECIDE_SERVICE_URL", decideApplication.GetEndpoint("http"))
    .WithEnvironment("INSPIRE_SERVICE_URL", inspireApplication.GetEndpoint("http"))
    .WithEnvironment("CHECKOUT_SERVICE_URL", checkoutApplication.GetEndpoint("http"));

builder.Build().Run();
