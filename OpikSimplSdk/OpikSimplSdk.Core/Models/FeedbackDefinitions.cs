using OpikSimplSdk.Core.Common;

namespace OpikSimplSdk.Core.Models;

public abstract record FeedbackCreate
{
    public required FeedbackDefinitionType Type { get; init; }
    public required string Name { get; init; }
}

public sealed record NumericalFeedbackCreate : FeedbackCreate
{
    public double? Min { get; init; }
    public double? Max { get; init; }
}

public sealed record CategoricalFeedbackCreate : FeedbackCreate
{
    public IReadOnlyList<string> Categories { get; init; } = [];
}

public abstract record FeedbackUpdate
{
    public required FeedbackDefinitionType Type { get; init; }
    public string? Name { get; init; }
}

public sealed record NumericalFeedbackUpdate : FeedbackUpdate
{
    public double? Min { get; init; }
    public double? Max { get; init; }
}

public sealed record CategoricalFeedbackUpdate : FeedbackUpdate
{
    public IReadOnlyList<string>? Categories { get; init; }
}

public abstract record FeedbackPublic
{
    public required string Id { get; init; }
    public required FeedbackDefinitionType Type { get; init; }
    public required string Name { get; init; }
}

public sealed record NumericalFeedbackPublic : FeedbackPublic
{
    public double? Min { get; init; }
    public double? Max { get; init; }
}

public sealed record CategoricalFeedbackPublic : FeedbackPublic
{
    public IReadOnlyList<string> Categories { get; init; } = [];
}
