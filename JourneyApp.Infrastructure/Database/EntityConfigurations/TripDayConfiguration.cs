using JourneyApp.Core.Models.Trip;
using JourneyApp.Core.ValueObjects.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyApp.Infrastructure.Database.EntityConfigurations;

public class TripDayConfiguration: IEntityTypeConfiguration<TripDay>
{
    public void Configure(EntityTypeBuilder<TripDay> builder)
    {
        builder.ToTable("TripDays");

        builder.HasKey(d => d.Id);

        builder.ComplexProperty(td => td.City, pb =>
        {
            pb.Property(c => c.Value)
                .HasMaxLength(City.MAX_CITY_LENGTH)
                .HasColumnName("City");
        });

        builder.HasMany(d => d.Places)
            .WithOne(p => p.TripDay)
            .HasForeignKey(p => p.TripDayId);
    }
}