using ChatApp.Chats.Domain.Chats.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Chats.Infrastructure.EntityConfigurations;

internal class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

        builder.Property(e => e.ChatId);
        builder.Property(e => e.SenderId);
        builder.Property(e => e.SentUTC);
        builder.Property(e => e.Content);
        builder.Property(e => e.Status).HasConversion<string>();
    }
}