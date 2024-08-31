using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Chat.Infrastructer.EntityConfigurations;

internal class ChatMessageConfiguration : IEntityTypeConfiguration<Domain.Entities.ChatMessage>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ChatMessage> builder)
    {
        builder.ToTable("ChatMessage");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

        builder.Property(e => e.ChatId);
        builder.Property(e => e.SenderId);
        builder.Property(e => e.SentUTC);
        builder.Property(e => e.Content);
        builder.Property(e => e.Status).HasConversion<string>();
    }
}