using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Http.Clients;

internal sealed class FeedbackDefinitionsClient : ClientBase, IFeedbackDefinitionsClient
{
    public FeedbackDefinitionsClient(IOpikHttpTransport transport) : base(transport)
    {
    }

    public Task<FeedbackDefinitionPagePublic> FindFeedbackDefinitionsAsync(int? page = null, int? size = null, string? name = null, FeedbackDefinitionType? type = null, RequestOptions? options = null)
        => Transport.SendAsync<FeedbackDefinitionPagePublic>(HttpMethod.Get, WithQuery("/v1/feedback-definitions", ("page", page), ("size", size), ("name", name), ("type", type)), options: options);

    public Task CreateFeedbackDefinitionAsync(FeedbackCreate request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/feedback-definitions", request, options);

    public Task<FeedbackPublic> GetFeedbackDefinitionByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync<FeedbackPublic>(HttpMethod.Get, $"/v1/feedback-definitions/{id}", options: options);

    public Task UpdateFeedbackDefinitionAsync(string id, FeedbackUpdate request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/feedback-definitions/{id}", request, options);

    public Task DeleteFeedbackDefinitionByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Delete, $"/v1/feedback-definitions/{id}", options: options);

    public Task DeleteFeedbackDefinitionsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/feedback-definitions/delete", new { ids }, options);
}
