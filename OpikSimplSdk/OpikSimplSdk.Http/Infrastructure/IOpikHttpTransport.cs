using OpikSimplSdk.Core.Common;

namespace OpikSimplSdk.Http.Infrastructure;

public interface IOpikHttpTransport
{
    Task<TResponse> SendAsync<TResponse>(HttpMethod method, string path, object? body = null, RequestOptions? options = null, CancellationToken cancellationToken = default);
    Task SendAsync(HttpMethod method, string path, object? body = null, RequestOptions? options = null, CancellationToken cancellationToken = default);
}
