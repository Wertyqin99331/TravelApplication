using CSharpFunctionalExtensions;
using JourneyApp.Application.Options;
using JourneyApp.Application.Services.FileStorageService;
using JourneyApp.Core.CommonTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace JourneyApp.Infrastructure.Services;

public class MinioService(IOptions<MinioOptions> options, IHttpContextAccessor httpContextAccessor) : IFileStorageService
{
    private readonly IMinioClient minioClient = new MinioClient()
        .WithEndpoint(options.Value.Endpoint)
        .WithCredentials(options.Value.AccessKey, options.Value.SecretKey)
        .Build();
    
    private readonly MinioOptions minioOptions = options.Value;
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
    
    private string GetPublicBaseUrl()
    {
        var request = httpContextAccessor.HttpContext?.Request;
        if (request == null) return string.Empty;
        
        return $"{request.Scheme}://{request.Host.Host}:{minioOptions.PublicPort}";
    }
    
    public async Task<Result<string, ApplicationError>> UploadFileAsync(
        IFormFile file,
        string fileName,
        string bucket,
        string? objectPath = null)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(bucket);
            
            var found = await minioClient.BucketExistsAsync(bucketExistsArgs);
            if (!found)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucket);
                
                await minioClient.MakeBucketAsync(makeBucketArgs);
                
                var policy = $@"{{
                    ""Version"": ""2012-10-17"",
                    ""Statement"": [
                        {{
                            ""Effect"": ""Allow"",
                            ""Principal"": {{
                                ""AWS"": [""*""]
                            }},
                            ""Action"": [""s3:GetObject""],
                            ""Resource"": [""arn:aws:s3:::{bucket}/*""]
                        }}
                    ]
                }}";
                
                await minioClient.SetPolicyAsync(new SetPolicyArgs()
                    .WithBucket(bucket)
                    .WithPolicy(policy));
            }

            var objectName = string.IsNullOrEmpty(objectPath) 
                ? fileName 
                : $"{objectPath.TrimEnd('/')}/{fileName}";

            using var stream = file.OpenReadStream();
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(file.ContentType);

            await minioClient.PutObjectAsync(putObjectArgs);
            
            var baseUrl = GetPublicBaseUrl();
            if (string.IsNullOrEmpty(baseUrl))
                return new ApplicationError("Could not determine public URL for MinIO");
                
            return $"{baseUrl}/{bucket}/{objectName}";
        }
        catch (Exception ex)
        {
            return new ApplicationError($"Failed to upload file: {ex.Message}");
        }
    }
}
