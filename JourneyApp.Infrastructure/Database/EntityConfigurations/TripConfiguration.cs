using JourneyApp.Core.Models.Trip;
using JourneyApp.Core.ValueObjects.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyApp.Infrastructure.Database.EntityConfigurations;

public class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.ToTable("Trips");

        builder.HasKey(t => t.Id);

        builder.ComplexProperty(t => t.City, propertyBuilder =>
        {
            propertyBuilder.Property(c => c.Value)
                .HasMaxLength(City.MAX_CITY_LENGTH)
                .HasColumnName("City");
        });

        builder.ComplexProperty(t => t.Description, propertyBuilder =>
        {
            propertyBuilder.Property(d => d.Value)
                .HasMaxLength(Description.MAX_DESCRIPTION_LENGTH)
                .HasColumnName("Description");
        });

        builder.ComplexProperty(t => t.Country, propertyBuilder =>
        {
            propertyBuilder.Property(c => c.Value)
                .HasMaxLength(Country.MAX_COUNTRY_LENGTH)
                .HasColumnName("Country");
        });

        builder.Ignore(t => t.AverageRating);
        builder.Ignore(t => t.OverallPrice);

        builder.HasMany(t => t.Days)
            .WithOne(d => d.Trip)
            .HasForeignKey(d => d.TripId);
    }
}