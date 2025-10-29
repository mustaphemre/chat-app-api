using ChatApp.Common;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// database
var sql = builder
    .AddSqlServer(name: ServiceNames.DATABASE.DATABASE_SERVER_NAME, port: 55600)
    .WithDataVolume();
var db = sql.AddDatabase(ServiceNames.DATABASE.DATABASE_NAME);

var kafka = builder.AddKafka("kafka", port: 55700)
    .WithKafkaUI();

// apis
var userApi = builder.AddProject<ChatApp_Users_Api>("chatapp-user-api")
    .WithReference(db)
    .WaitFor(db)
    .WithExternalHttpEndpoints();

builder.AddProject<ChatApp_Chats_Api>("chatapp-chat-api")
    .WithReference(db)
    .WithReference(userApi)
    .WithReference(kafka)
    .WaitFor(db)
    .WaitFor(kafka)
    .WithExternalHttpEndpoints();

builder.AddProject<ChatApp_Chats_Worker>("chatapp-chats-worker")
    .WithReference(kafka)
    .WithReference(db)
    .WaitFor(kafka)
    .WaitFor(db);

builder.AddProject<ChatApp_Worker_DbMigration>("chatapp-worker-dbmigration")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();