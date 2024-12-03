using CSharpFunctionalExtensions;
using JourneyApp.Application.Services.Authentication.Dto;
using JourneyApp.Core.CommonTypes;


namespace JourneyApp.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<Result<LoginResult, ApplicationError>> LoginAsync(LoginBody body);
    Task<UnitResult<ApplicationError>> RegisterAsync(RegisterBody body);
    Task<UnitResult<ApplicationError>> UpdateProfileAsync(UpdateProfileBody body);
    Task<Result<GetProfileResult, ApplicationError>> GetProfileAsync();
}