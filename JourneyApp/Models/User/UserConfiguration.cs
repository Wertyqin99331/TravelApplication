using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyApp.Models.User;

public class UserConfiguration : IEntityTypeConfiguration<global::User>
{
    public void Configure(EntityTypeBuilder<global::User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(u => u.TripRequests)
            .WithOne(tr => tr.User)
            .HasForeignKey(tr => tr.UserId);

        builder.HasMany(u => u.Trips)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);
    }
}