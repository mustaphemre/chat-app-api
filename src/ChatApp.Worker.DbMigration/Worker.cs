using ChatApp.Chats.Infrastructure;
using ChatApp.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ChatApp.Worker.DbMigration;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<Worker> _logger;

    internal const string ActivityName = "MigrationService";
    private static readonly ActivitySource _activitySource = new(ActivityName);

    public Worker(
        IServiceProvider serviceProvider,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = _activitySource.StartActivity("Migrating database", ActivityKind.Client);

        await MigrateChatDbAsync(activity, stoppingToken);
        await MigrateUserDbAsync(activity, stoppingToken);

        _hostApplicationLifetime.StopApplication();
    }

    private async Task MigrateChatDbAsync(Activity? activity, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
            await dbContext.Database.MigrateAsync(stoppingToken);

        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }
    }

    private async Task MigrateUserDbAsync(Activity? activity, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            await dbContext.Database.MigrateAsync(stoppingToken);

        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }
    }
}