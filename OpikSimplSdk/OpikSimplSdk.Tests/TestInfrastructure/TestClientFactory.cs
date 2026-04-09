using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Http;
using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Tests.TestInfrastructure;

internal static class TestClientFactory
{
    public static (OpikClient Client, RecordingMessageHandler Handler) CreateOpikClient(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>? responder = null,
        AuthHeaderMode authHeaderMode = AuthHeaderMode.AuthorizationBearer)
    {
        var handler = new RecordingMessageHandler(responder);
        var httpClient = new HttpClient(handler);
        var config = new OpikClientConfig
        {
            BaseUrl = "https://api.opik.test",
            ApiKey = "test-api-key",
            WorkspaceName = "workspace"
        };

        var client = new OpikClient(config, httpClient, authHeaderMode);
        return (client, handler);
    }
}
