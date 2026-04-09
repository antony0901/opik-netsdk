namespace OpikSimplSdk.Core.Common;

public sealed record RequestOptions
{
    public TimeSpan? Timeout { get; init; }
    public int? ChunkSize { get; init; }
}
