using ChatApp.Chats.Worker.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddConsumerServices();

var host = builder.Build();
host.Run();