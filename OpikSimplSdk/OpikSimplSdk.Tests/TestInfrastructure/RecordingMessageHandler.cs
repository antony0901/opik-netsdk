using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace OpikSimplSdk.Tests.TestInfrastructure;

internal sealed class RecordingMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _responder;

    public RecordingMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>? responder = null)
    {
        _responder = responder ?? DefaultResponderAsync;
    }

    public List<CapturedRequest> Requests { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
        var headers = request.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value));
        if (request.Content is not null)
        {
            foreach (var header in request.Content.Headers)
            {
                headers[header.Key] = string.Join(",", header.Value);
            }
        }

        Requests.Add(new CapturedRequest(
            request.Method,
            request.RequestUri?.PathAndQuery ?? string.Empty,
            body,
            headers));

        return await _responder(request, cancellationToken);
    }

    private static Task<HttpResponseMessage> DefaultResponderAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var content = "{}";
        if (request.RequestUri?.AbsolutePath.Contains("/names", StringComparison.OrdinalIgnoreCase) == true)
        {
            content = "[]";
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        });
    }
}

internal sealed record CapturedRequest(HttpMethod Method, string PathAndQuery, string? Body, IReadOnlyDictionary<string, string> Headers)
{
    public bool TryGetHeader(string name, out string value)
    {
        if (Headers.TryGetValue(name, out value!))
        {
            return true;
        }

        var match = Headers.FirstOrDefault(x => string.Equals(x.Key, name, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(match.Key))
        {
            value = match.Value;
            return true;
        }

        value = string.Empty;
        return false;
    }
}
