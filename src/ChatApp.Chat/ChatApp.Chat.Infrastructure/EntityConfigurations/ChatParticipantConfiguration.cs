using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Chat.Infrastructer.EntityConfigurations;

internal class ChatParticipantConfiguration : IEntityTypeConfiguration<Domain.Entities.ChatParticipant>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ChatParticipant> builder)
    {
        builder.ToTable("ChatParticipant");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

        builder.Property(e => e.ChatId);
        builder.Property(e => e.UserId);
        builder.Property(e => e.JoinedUTC);
    }
}