namespace OpikSimplSdk.Core.Common;

public sealed record OpikClientConfig
{
    public required string BaseUrl { get; init; }
    public required string ApiKey { get; init; }
    public string? WorkspaceName { get; init; }
}
