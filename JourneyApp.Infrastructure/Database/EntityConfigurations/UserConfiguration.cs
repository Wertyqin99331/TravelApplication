using JourneyApp.Core.ValueObjects.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyApp.Infrastructure.Database.EntityConfigurations;

public class UserConfiguration: IEntityTypeConfiguration<Core.Models.User.User>
{
    public void Configure(EntityTypeBuilder<Core.Models.User.User> builder)
    {
        builder.HasKey(u => u.Id);

        builder
            .ComplexProperty(u => u.Name, pb =>
            {
                pb.Property(n => n.Value)
                    .HasMaxLength(Name.MAX_NAME_LENGTH)
                    .HasColumnName("Name");
            });
        
        builder
            .ComplexProperty(u => u.Surname, pb =>
            {
                pb.Property(sn => sn.Value)
                    .HasMaxLength(Name.MAX_NAME_LENGTH)
                    .HasColumnName("Surname");
            });

        builder.HasMany(u => u.TripReviews)
            .WithOne(tr => tr.User)
            .HasForeignKey(tr => tr.UserId);
    }
}