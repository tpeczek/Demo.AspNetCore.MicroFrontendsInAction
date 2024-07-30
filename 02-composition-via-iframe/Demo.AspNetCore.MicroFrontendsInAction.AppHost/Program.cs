var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Decide>("decide");
builder.AddProject<Projects.Demo_AspNetCore_MicroFrontendsInAction_Inspire>("inspire");

builder.Build().Run();
