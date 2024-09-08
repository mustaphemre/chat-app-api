using ChatApp.Chat.Domain.Chat.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Chat.Infrastructure.EntityConfigurations;

internal class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> builder)
    {
        builder.ToTable("ChatParticipant");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

        builder.Property(e => e.ChatId);
        builder.Property(e => e.UserId);
        builder.Property(e => e.JoinedUTC);
    }
}