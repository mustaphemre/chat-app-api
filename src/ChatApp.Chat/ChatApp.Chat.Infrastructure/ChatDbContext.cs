using ChatApp.Chat.Domain.Chat.Entities;
using ChatApp.Chat.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Chat.Infrastructure;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Chat.Chat> Chats { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}