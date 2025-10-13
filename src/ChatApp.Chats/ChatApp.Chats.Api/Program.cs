using ChatApp.Chats.Application;
using ChatApp.Chats.Application.Extensions;
using ChatApp.Chats.Consumers.Extensions;
using ChatApp.Chats.Infrastructure;
using ChatApp.Chats.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddChatApiInfrastructureServices();
builder.AddChatApplicationServices();
builder.AddChatConsumerServices();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(opt =>
{
    opt.RegisterServicesFromAssembly(ApplicationAssembly.Instance);
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.AddChatsEndpoints();
app.AddMessagesEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.CreateDbIfNotExists();

app.Run();