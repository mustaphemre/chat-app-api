using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Chat.Infrastructure.EntityConfigurations;

internal class ChatConfiguration : IEntityTypeConfiguration<Domain.Chat.Chat>
{
    public void Configure(EntityTypeBuilder<Domain.Chat.Chat> builder)
    {
        builder.ToTable("Chat");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

        builder.Property(e => e.IsGroupChat);
        builder.Property(e => e.CreatedUTC).HasDefaultValueSql("getdate()");
        builder.Property(e => e.LastUpdatedUTC).IsRequired(false);

        builder
            .HasMany(e => e.ChatParticipants)
            .WithOne()
            .HasForeignKey(e => e.ChatId);

        builder
            .HasMany(e => e.ChatMessages)
            .WithOne()
            .HasForeignKey(e => e.ChatId);
    }
}
