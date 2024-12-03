namespace JourneyApp.Application.Options;

public class MinioOptions
{
    public const string SECTION_NAME = "MinioOptions";
    
    public string Endpoint { get; init; } = null!;
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public string DefaultBucket { get; init; } = null!;
    public int PublicPort { get; init; }
}
