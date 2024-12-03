using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using JourneyApp.Core.Models.User;

namespace JourneyApp.Application.Services.UserService;

public interface IUserService
{
    Task<Result<User, ApplicationError>> GetUserFromTokenAsync();
}
