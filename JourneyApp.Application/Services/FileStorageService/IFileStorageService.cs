using CSharpFunctionalExtensions;
using JourneyApp.Core.CommonTypes;
using Microsoft.AspNetCore.Http;

namespace JourneyApp.Application.Services.FileStorageService;

public interface IFileStorageService
{
    Task<Result<string, ApplicationError>> UploadFileAsync(
        IFormFile file,
        string fileName,
        string bucket,
        string? objectPath = null);
}
