using ChatApp.Chat.Application;
using ChatApp.Chat.Infrastructure;
using ChatApp.Chat.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddChatApiInfrastructureServices();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



//app.CreateDbIfNotExists();

app.Run();