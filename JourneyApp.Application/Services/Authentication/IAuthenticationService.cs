using CSharpFunctionalExtensions;
using JourneyApp.Application.Services.Authentication.Dto;
using JourneyApp.Core.CommonTypes;

namespace JourneyApp.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<UnitResult<ApplicationError>> RegisterAsync(RegisterBody body);
    Task<Result<LoginResult, ApplicationError>> LoginAsync(LoginBody body);
}