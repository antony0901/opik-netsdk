using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using OpikSimplSdk.Core.Common;

namespace OpikSimplSdk.Http.Infrastructure;

public sealed class OpikHttpTransport : IOpikHttpTransport
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public OpikHttpTransport(HttpClient httpClient, OpikClientConfig config, AuthHeaderMode authHeaderMode = AuthHeaderMode.AuthorizationBearer, JsonSerializerOptions? jsonOptions = null)
    {
        _httpClient = httpClient;
        _jsonOptions = jsonOptions ?? OpikJson.Default;

        if (_httpClient.BaseAddress is null)
        {
            _httpClient.BaseAddress = new Uri(config.BaseUrl);
        }

        if (authHeaderMode == AuthHeaderMode.AuthorizationBearer)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.ApiKey);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Remove("comet-sdk-api-key");
            _httpClient.DefaultRequestHeaders.Add("comet-sdk-api-key", config.ApiKey);
        }
    }

    public async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string path, object? body = null, RequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        using var response = await SendCoreAsync(method, path, body, options, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var payload = await JsonSerializer.DeserializeAsync<TResponse>(stream, _jsonOptions, cancellationToken).ConfigureAwait(false);
        return payload ?? throw new InvalidOperationException($"Response body was empty for '{path}'.");
    }

    public async Task SendAsync(HttpMethod method, string path, object? body = null, RequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        using var response = await SendCoreAsync(method, path, body, options, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    private async Task<HttpResponseMessage> SendCoreAsync(HttpMethod method, string path, object? body, RequestOptions? options, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, path);

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        if (options?.Timeout is { } timeout)
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(timeout);
            return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeoutCts.Token).ConfigureAwait(false);
        }

        return await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
    }
}
