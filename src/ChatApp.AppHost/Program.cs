var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ChatApp_Chat_Api>("chatapp-chat-api");

builder.Build().Run();
