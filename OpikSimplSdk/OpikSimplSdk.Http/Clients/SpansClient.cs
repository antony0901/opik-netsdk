using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Http.Clients;

internal sealed class SpansClient : ClientBase, ISpansClient
{
    public SpansClient(IOpikHttpTransport transport) : base(transport)
    {
    }

    public Task CreateSpanAsync(CreateSpanRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/spans", request, options);

    public Task CreateSpansAsync(IEnumerable<SpanWrite> spans, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/spans/batch", spans, options);

    public Task<SpanPublic> GetSpanByIdAsync(string id, bool? stripAttachments = null, RequestOptions? options = null)
        => Transport.SendAsync<SpanPublic>(HttpMethod.Get, WithQuery($"/v1/spans/{id}", ("stripAttachments", stripAttachments)), options: options);

    public Task UpdateSpanAsync(string id, UpdateSpanRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/spans/{id}", request, options);

    public Task DeleteSpanByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Delete, $"/v1/spans/{id}", options: options);

    public Task<SpanPagePublic> GetSpansByProjectAsync(GetSpansRequest request, RequestOptions? options = null)
        => Transport.SendAsync<SpanPagePublic>(HttpMethod.Post, "/v1/spans/find", request, options);

    public IAsyncEnumerable<byte[]> SearchSpansAsync(SearchSpansRequest request, RequestOptions? options = null)
        => Transport.StreamBytesAsync(HttpMethod.Post, "/v1/spans/search", request, options);

    public Task AddSpanFeedbackScoreAsync(string id, FeedbackScoreRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, $"/v1/spans/{id}/feedback-scores", request, options);

    public Task DeleteSpanFeedbackScoreAsync(string id, string name, string? author = null, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Delete, WithQuery($"/v1/spans/{id}/feedback-scores/{name}", ("author", author)), options: options);

    public Task ScoreBatchOfSpansAsync(IEnumerable<FeedbackScoreBatchItem> scores, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/spans/feedback-scores/batch", scores, options);

    public Task<IList<string>> FindFeedbackScoreNamesAsync(string? projectId = null, SpanType? type = null, RequestOptions? options = null)
        => Transport.SendAsync<IList<string>>(HttpMethod.Get, WithQuery("/v1/spans/feedback-scores/names", ("projectId", projectId), ("type", type)), options: options);

    public Task<ProjectStatsPublic> GetSpanStatsAsync(string? projectId = null, string? projectName = null, string? traceId = null, SpanType? type = null, string? filters = null, RequestOptions? options = null)
        => Transport.SendAsync<ProjectStatsPublic>(HttpMethod.Get, WithQuery("/v1/spans/stats", ("projectId", projectId), ("projectName", projectName), ("traceId", traceId), ("type", type), ("filters", filters)), options: options);

    public Task AddSpanCommentAsync(string spanId, CommentRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, $"/v1/spans/{spanId}/comments", request, options);

    public Task<Comment> GetSpanCommentAsync(string commentId, string spanId, RequestOptions? options = null)
        => Transport.SendAsync<Comment>(HttpMethod.Get, $"/v1/spans/{spanId}/comments/{commentId}", options: options);

    public Task UpdateSpanCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/spans/comments/{commentId}", request, options);

    public Task DeleteSpanCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/spans/comments/delete", new { ids }, options);
}
