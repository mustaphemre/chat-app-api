using Microsoft.EntityFrameworkCore;

namespace ChatApp.Chat.Infrastructer;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Entities.Chat> Chats { get; set; }
    public DbSet<Domain.Entities.ChatMessage> ChatMessages { get; set; }
    public DbSet<Domain.Entities.ChatParticipant> ChatParticipants { get; set; }
    public DbSet<Domain.Entities.User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}