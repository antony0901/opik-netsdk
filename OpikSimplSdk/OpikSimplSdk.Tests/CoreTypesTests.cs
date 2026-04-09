using System.Text.Json;
using OpikSimplSdk.Core.Common;

namespace OpikSimplSdk.Tests;

public sealed class CoreTypesTests
{
    [Fact]
    public void Optional_ShouldBeUnsetByDefault()
    {
        Optional<string> optional = default;

        Assert.False(optional.IsSet);
        Assert.Null(optional.Value);
    }

    [Fact]
    public void Optional_ShouldBeSetWhenAssigned()
    {
        Optional<string> optional = "value";

        Assert.True(optional.IsSet);
        Assert.Equal("value", optional.Value);
    }

    [Fact]
    public void Optional_ShouldSerializeSetValue()
    {
        Optional<string> optional = "hello";

        var json = JsonSerializer.Serialize(optional, new JsonSerializerOptions { Converters = { new OptionalJsonConverterFactory() } });

        Assert.Equal("\"hello\"", json);
    }

    [Fact]
    public void RequestOptions_ShouldStoreValues()
    {
        var options = new RequestOptions { Timeout = TimeSpan.FromSeconds(2), ChunkSize = 512 };

        Assert.Equal(TimeSpan.FromSeconds(2), options.Timeout);
        Assert.Equal(512, options.ChunkSize);
    }
}
