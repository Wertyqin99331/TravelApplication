using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyApp.Models.Trip;

public class TripConfiguration : IEntityTypeConfiguration<global::Trip>
{
    public void Configure(EntityTypeBuilder<global::Trip> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Destination)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.StartDate)
            .IsRequired();

        builder.Property(t => t.EndDate)
            .IsRequired();

        builder.HasMany(t => t.TripRequests)
            .WithOne(tr => tr.Trip)
            .HasForeignKey(tr => tr.TripId);
    }
}