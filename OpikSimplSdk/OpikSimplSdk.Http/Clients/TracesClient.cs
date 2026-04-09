using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Http.Clients;

internal sealed class TracesClient : ClientBase, ITracesClient
{
    public TracesClient(IOpikHttpTransport transport) : base(transport)
    {
    }

    public Task CreateTraceAsync(CreateTraceRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/traces", request, options);

    public Task CreateTracesAsync(IEnumerable<TraceWrite> traces, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/traces/batch", traces, options);

    public Task<TracePublic> GetTraceByIdAsync(string id, bool? stripAttachments = null, RequestOptions? options = null)
        => Transport.SendAsync<TracePublic>(HttpMethod.Get, WithQuery($"/v1/traces/{id}", ("stripAttachments", stripAttachments)), options: options);

    public Task UpdateTraceAsync(string id, UpdateTraceRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/traces/{id}", request, options);

    public Task DeleteTraceByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Delete, $"/v1/traces/{id}", options: options);

    public Task DeleteTracesAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/traces/delete", new { ids }, options);

    public Task<TracePagePublic> GetTracesByProjectAsync(GetTracesRequest request, RequestOptions? options = null)
        => Transport.SendAsync<TracePagePublic>(HttpMethod.Post, "/v1/traces/find", request, options);

    public IAsyncEnumerable<byte[]> SearchTracesAsync(SearchTracesRequest request, RequestOptions? options = null)
        => Transport.StreamBytesAsync(HttpMethod.Post, "/v1/traces/search", request, options);

    public Task AddTraceFeedbackScoreAsync(string id, FeedbackScoreRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, $"/v1/traces/{id}/feedback-scores", request, options);

    public Task DeleteTraceFeedbackScoreAsync(string id, string name, string? author = null, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Delete, WithQuery($"/v1/traces/{id}/feedback-scores/{name}", ("author", author)), options: options);

    public Task ScoreBatchOfTracesAsync(IEnumerable<FeedbackScoreBatchItem> scores, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/traces/feedback-scores/batch", scores, options);

    public Task<IList<string>> FindFeedbackScoreNamesAsync(string? projectId = null, RequestOptions? options = null)
        => Transport.SendAsync<IList<string>>(HttpMethod.Get, WithQuery("/v1/traces/feedback-scores/names", ("projectId", projectId)), options: options);

    public Task<ProjectStatsPublic> GetTraceStatsAsync(string? projectId = null, string? projectName = null, string? filters = null, RequestOptions? options = null)
        => Transport.SendAsync<ProjectStatsPublic>(HttpMethod.Get, WithQuery("/v1/traces/stats", ("projectId", projectId), ("projectName", projectName), ("filters", filters)), options: options);

    public Task AddTraceCommentAsync(string traceId, CommentRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, $"/v1/traces/{traceId}/comments", request, options);

    public Task<Comment> GetTraceCommentAsync(string commentId, string traceId, RequestOptions? options = null)
        => Transport.SendAsync<Comment>(HttpMethod.Get, $"/v1/traces/{traceId}/comments/{commentId}", options: options);

    public Task UpdateTraceCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/traces/comments/{commentId}", request, options);

    public Task DeleteTraceCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/traces/comments/delete", new { ids }, options);

    public Task<TraceThread> GetTraceThreadAsync(GetTraceThreadRequest request, RequestOptions? options = null)
        => Transport.SendAsync<TraceThread>(HttpMethod.Post, "/v1/trace-threads/get", request, options);

    public Task<TraceThreadPage> GetTraceThreadsAsync(GetTraceThreadsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<TraceThreadPage>(HttpMethod.Post, "/v1/trace-threads/find", request, options);

    public IAsyncEnumerable<byte[]> SearchTraceThreadsAsync(SearchTraceThreadsRequest request, RequestOptions? options = null)
        => Transport.StreamBytesAsync(HttpMethod.Post, "/v1/trace-threads/search", request, options);

    public Task OpenTraceThreadAsync(string threadId, string? projectName = null, string? projectId = null, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, $"/v1/trace-threads/{threadId}/open", new { projectName, projectId }, options);

    public Task CloseTraceThreadAsync(CloseTraceThreadRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/trace-threads/close", request, options);

    public Task UpdateThreadAsync(string threadModelId, IEnumerable<string>? tags = null, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/trace-threads/{threadModelId}", new { tags }, options);

    public Task DeleteTraceThreadsAsync(IEnumerable<string> threadIds, string? projectName = null, string? projectId = null, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/trace-threads/delete", new { threadIds, projectName, projectId }, options);

    public Task ScoreBatchOfThreadsAsync(IEnumerable<FeedbackScoreBatchItemThread> scores, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/trace-threads/feedback-scores/batch", scores, options);

    public Task<IList<string>> FindTraceThreadsFeedbackScoreNamesAsync(string projectId, RequestOptions? options = null)
        => Transport.SendAsync<IList<string>>(HttpMethod.Get, WithQuery("/v1/trace-threads/feedback-scores/names", ("projectId", projectId)), options: options);

    public Task DeleteThreadFeedbackScoresAsync(string projectName, string threadId, IEnumerable<string> names, string? author = null, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/trace-threads/feedback-scores/delete", new { projectName, threadId, names, author }, options);

    public Task AddThreadCommentAsync(string threadId, CommentRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, $"/v1/trace-threads/{threadId}/comments", request, options);

    public Task<Comment> GetThreadCommentAsync(string commentId, string threadId, RequestOptions? options = null)
        => Transport.SendAsync<Comment>(HttpMethod.Get, $"/v1/trace-threads/{threadId}/comments/{commentId}", options: options);

    public Task UpdateThreadCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/trace-threads/comments/{commentId}", request, options);

    public Task DeleteThreadCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/trace-threads/comments/delete", new { ids }, options);
}
