using ChatApp.Common;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// database
var sql = builder
    .AddSqlServer(name: ServiceNames.DATABASE.DATABASE_SERVER_NAME, port: 54600)
    .WithDataVolume();
var db = sql.AddDatabase(ServiceNames.DATABASE.DATABASE_NAME);

// apis
builder
    .AddProject<ChatApp_Chat_Api>("chatapp-chat-api")
    .WithReference(db)
    .WithExternalHttpEndpoints();

builder.AddProject<ChatApp_Worker_DbMigration>("chatapp-worker-dbmigration")
    .WithReference(db);

builder.AddProject<ChatApp_Users_Api>("chatapp-user-api");

builder.Build().Run();