using System.Net;
using System.Text;
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Http.Infrastructure;
using OpikSimplSdk.Tests.TestInfrastructure;

namespace OpikSimplSdk.Tests;

public sealed class OpikHttpTransportTests
{
    [Fact]
    public async Task SendAsyncGeneric_ShouldDeserializePayload()
    {
        var (transport, handler) = CreateTransport((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        }));

        var result = await transport.SendAsync<DummyPayload>(HttpMethod.Get, "/v1/test");

        Assert.NotNull(result);
        Assert.Single(handler.Requests);
        Assert.Equal("/v1/test", handler.Requests[0].PathAndQuery);
    }

    [Fact]
    public async Task SendAsyncVoid_ShouldSendBodyAsJson()
    {
        var (transport, handler) = CreateTransport();

        await transport.SendAsync(HttpMethod.Post, "/v1/test", new { value = "x" });

        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("/v1/test", request.PathAndQuery);
        Assert.Contains("\"value\":\"x\"", request.Body);
        Assert.True(request.TryGetHeader("Content-Type", out var contentType));
        Assert.Contains("application/json", contentType);
    }

    [Fact]
    public async Task StreamBytesAsync_ShouldReturnNonBlankLines()
    {
        var payload = "{\"a\":1}\n\n{\"b\":2}\n";
        var (transport, _) = CreateTransport((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/x-ndjson")
        }));

        var items = new List<string>();
        await foreach (var bytes in transport.StreamBytesAsync(HttpMethod.Get, "/v1/stream"))
        {
            items.Add(Encoding.UTF8.GetString(bytes));
        }

        Assert.Equal(2, items.Count);
        Assert.Equal("{\"a\":1}", items[0]);
        Assert.Equal("{\"b\":2}", items[1]);
    }

    [Fact]
    public async Task Constructor_ShouldApplyAuthorizationBearerHeader()
    {
        var (transport, handler) = CreateTransport(authHeaderMode: AuthHeaderMode.AuthorizationBearer);

        await transport.SendAsync(HttpMethod.Get, "/v1/test");

        var request = Assert.Single(handler.Requests);
        Assert.True(request.TryGetHeader("Authorization", out var authHeader));
        Assert.Equal("Bearer test-api-key", authHeader);
    }

    [Fact]
    public async Task Constructor_ShouldApplyCometApiKeyHeader()
    {
        var (transport, handler) = CreateTransport(authHeaderMode: AuthHeaderMode.CometSdkApiKey);

        await transport.SendAsync(HttpMethod.Get, "/v1/test");

        var request = Assert.Single(handler.Requests);
        Assert.True(request.TryGetHeader("comet-sdk-api-key", out var apiKey));
        Assert.Equal("test-api-key", apiKey);
    }

    [Fact]
    public async Task SendAsync_ShouldRespectRequestTimeout()
    {
        var (transport, _) = CreateTransport(async (_, cancellationToken) =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };
        });

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            transport.SendAsync(HttpMethod.Get, "/v1/test", options: new RequestOptions { Timeout = TimeSpan.FromMilliseconds(5) }));
    }

    private static (OpikHttpTransport Transport, RecordingMessageHandler Handler) CreateTransport(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>? responder = null,
        AuthHeaderMode authHeaderMode = AuthHeaderMode.AuthorizationBearer)
    {
        var handler = new RecordingMessageHandler(responder);
        var httpClient = new HttpClient(handler);
        var config = new OpikClientConfig
        {
            BaseUrl = "https://api.opik.test",
            ApiKey = "test-api-key"
        };

        return (new OpikHttpTransport(httpClient, config, authHeaderMode), handler);
    }

    private sealed record DummyPayload;
}
