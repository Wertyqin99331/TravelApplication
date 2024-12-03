using JourneyApp.Core.Models.TripReview;
using Mapster;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace JourneyApp.Application.Dto.Trip;

public record TripReviewDto(
    Guid Id,
    int Rating,
    string ReviewText,
    DateOnly Date,
    Guid UserId,
    string UserName,
    string UserSurname,
    string? UserAvatarUrl);

public class TripReviewDtoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TripReview, TripReviewDto>();
    }
}
