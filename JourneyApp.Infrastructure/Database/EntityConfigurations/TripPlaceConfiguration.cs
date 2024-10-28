using JourneyApp.Core.Models.Trip;
using JourneyApp.Core.ValueObjects.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyApp.Infrastructure.Database.EntityConfigurations;

public class TripPlaceConfiguration: IEntityTypeConfiguration<TripPlace>
{
    public void Configure(EntityTypeBuilder<TripPlace> builder)
    {
        builder.ToTable("TripPlaces").HasKey(p => p.Id);

        builder.ComplexProperty(tp => tp.Title, pb =>
        {
            pb.Property(t => t.Value)
                .HasMaxLength(Title.MAX_TITLE_LENGTH)
                .HasColumnName("Title");
        });

        builder.ComplexProperty(tp => tp.Description, pb =>
        {
            pb.Property(d => d.Value)
                .HasMaxLength(Description.MAX_DESCRIPTION_LENGTH)
                .HasColumnName("Description");
        });
    }
}