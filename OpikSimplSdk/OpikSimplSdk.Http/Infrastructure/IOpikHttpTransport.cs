using OpikSimplSdk.Core.Common;

namespace OpikSimplSdk.Http.Infrastructure;

/// <summary>
/// Defines shared HTTP transport primitives for Opik SDK clients.
/// </summary>
public interface IOpikHttpTransport
{
    /// <summary>Sends a request and deserializes the response payload.</summary>
    Task<TResponse> SendAsync<TResponse>(HttpMethod method, string path, object? body = null, RequestOptions? options = null, CancellationToken cancellationToken = default);
    /// <summary>Sends a request without expecting a response payload.</summary>
    Task SendAsync(HttpMethod method, string path, object? body = null, RequestOptions? options = null, CancellationToken cancellationToken = default);
    /// <summary>Streams response content as byte chunks (line-oriented for NDJSON endpoints).</summary>
    IAsyncEnumerable<byte[]> StreamBytesAsync(HttpMethod method, string path, object? body = null, RequestOptions? options = null, CancellationToken cancellationToken = default);
}
