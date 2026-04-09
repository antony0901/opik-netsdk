using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Http.Clients;

internal sealed class PromptsClient : ClientBase, IPromptsClient
{
    public PromptsClient(IOpikHttpTransport transport) : base(transport)
    {
    }

    public Task<PromptPagePublic> GetPromptsAsync(GetPromptsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<PromptPagePublic>(HttpMethod.Post, "/v1/prompts/find", request, options);

    public Task CreatePromptAsync(CreatePromptRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/prompts", request, options);

    public Task<PromptDetail> GetPromptByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync<PromptDetail>(HttpMethod.Get, $"/v1/prompts/{id}", options: options);

    public Task UpdatePromptAsync(string id, UpdatePromptRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/prompts/{id}", request, options);

    public Task DeletePromptAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Delete, $"/v1/prompts/{id}", options: options);

    public Task DeletePromptsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/prompts/delete", new { ids }, options);

    public Task<PromptVersionDetail> CreatePromptVersionAsync(string name, PromptVersionDetail version, RequestOptions? options = null)
        => Transport.SendAsync<PromptVersionDetail>(HttpMethod.Post, WithQuery("/v1/prompt-versions", ("name", name)), version, options);

    public Task<PromptVersionDetail> GetPromptVersionByIdAsync(string versionId, RequestOptions? options = null)
        => Transport.SendAsync<PromptVersionDetail>(HttpMethod.Get, $"/v1/prompt-versions/{versionId}", options: options);

    public Task<PromptVersionPagePublic> GetPromptVersionsAsync(string id, int? page = null, int? size = null, RequestOptions? options = null)
        => Transport.SendAsync<PromptVersionPagePublic>(HttpMethod.Get, WithQuery($"/v1/prompts/{id}/versions", ("page", page), ("size", size)), options: options);

    public Task<PromptVersionDetail> RetrievePromptVersionAsync(string name, string? commit = null, RequestOptions? options = null)
        => Transport.SendAsync<PromptVersionDetail>(HttpMethod.Get, WithQuery("/v1/prompt-versions/retrieve", ("name", name), ("commit", commit)), options: options);

    public Task<PromptVersionDetail> RestorePromptVersionAsync(string promptId, string versionId, RequestOptions? options = null)
        => Transport.SendAsync<PromptVersionDetail>(HttpMethod.Post, $"/v1/prompts/{promptId}/versions/{versionId}/restore", options: options);
}
