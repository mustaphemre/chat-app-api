using ChatApp.Chats.Infrastructure;
using ChatApp.Users.Infrastructure;
using ChatApp.Worker.DbMigration;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.AddChatApiInfrastructureServices();
builder.AddUserApiInfrastructureServices();

builder.Services.AddOpenTelemetry()
    .WithTracing(c => c.AddSource(Worker.ActivityName));

var host = builder.Build();
host.Run();
