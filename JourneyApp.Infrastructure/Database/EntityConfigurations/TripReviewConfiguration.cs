using JourneyApp.Core.Models.TripReview;
using JourneyApp.Core.ValueObjects.TripReview;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyApp.Infrastructure.Database.EntityConfigurations;

public class TripReviewConfiguration: IEntityTypeConfiguration<TripReview>
{
    public void Configure(EntityTypeBuilder<TripReview> builder)
    {
        builder.ToTable("TripReviews").HasKey(tr => tr.Id);

        builder.ComplexProperty(tr => tr.ReviewText, pb =>
        {
            pb.Property(rt => rt.Value)
                .HasMaxLength(ReviewText.MAX_REVIEW_TEXT_LENGTH)
                .HasColumnName("ReviewText");
        });


        builder.ComplexProperty(tr => tr.Rating, pb =>
        {
            pb.Property(r => r.Value)
                .HasColumnName("Rating");
        });
    }
}