using ChatApp.Users.Application;
using ChatApp.Users.Infrastructure;
using ChatApp.Users.Presentation.Endpoints;
using ChatApp.Users.Presentation.GrpcServices;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddUserApiInfrastructureServices();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpc().AddJsonTranscoding();

builder.Services.AddMediatR(opt =>
{
    opt.RegisterServicesFromAssembly(ApplicationAssembly.Instance);
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.AddUsersEndpoints();
app.MapGrpcService<UsersGrpcService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.CreateDbIfNotExists();

app.Run();