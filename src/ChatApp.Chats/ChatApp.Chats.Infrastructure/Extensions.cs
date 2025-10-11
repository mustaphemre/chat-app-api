using ChatApp.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatApp.Chats.Infrastructure;

public static class Extensions
{
    public static IHostApplicationBuilder AddChatApiInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<ChatDbContext>(ServiceNames.DATABASE.DATABASE_NAME);

        return builder;
    }

    public static void CreateDbIfNotExists(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ChatDbContext>();

        var tst = context.Database;
        var cs = tst.GetConnectionString();
        try
        {
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            throw new Exception("Something went wong on creating database", ex);
        }
    }
}