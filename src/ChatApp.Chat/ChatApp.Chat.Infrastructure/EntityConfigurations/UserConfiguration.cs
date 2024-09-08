using ChatApp.Chat.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Chat.Infrastructure.EntityConfigurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
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
            .WithOne()
            .HasForeignKey(e => e.UserId);

        builder
            .HasMany(e => e.ChatMessages)
            .WithOne()
            .HasForeignKey(e => e.SenderId);
    }
}