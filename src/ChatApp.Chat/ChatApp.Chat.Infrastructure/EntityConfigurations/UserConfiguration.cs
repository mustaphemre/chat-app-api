using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Chat.Infrastructer.EntityConfigurations;

internal class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.User>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

        builder.Property(e => e.Username).IsRequired();
        builder.Property(e => e.Email).IsRequired();
        builder.Property(e => e.Status).HasConversion<int>();
        builder.Property(e => e.ProfilePicture);

        builder
            .HasMany(e => e.ChatParticipations)
            .WithOne(e => e.ChatUser)
            .HasForeignKey(e => e.UserId);

        builder
            .HasMany(e => e.ChatMessages)
            .WithOne(e => e.SenderUser)
            .HasForeignKey(e => e.SenderId);
    }
}