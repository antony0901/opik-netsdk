using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Http.Clients;

internal sealed class ProjectsClient : ClientBase, IProjectsClient
{
    public ProjectsClient(IOpikHttpTransport transport) : base(transport)
    {
    }

    public Task<ProjectPagePublic> FindProjectsAsync(FindProjectsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<ProjectPagePublic>(HttpMethod.Post, "/v1/projects/find", request, options);

    public Task CreateProjectAsync(CreateProjectRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/projects", request, options);

    public Task<ProjectPublic> GetProjectByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync<ProjectPublic>(HttpMethod.Get, $"/v1/projects/{id}", options: options);

    public Task<ProjectDetailed> RetrieveProjectAsync(string name, RequestOptions? options = null)
        => Transport.SendAsync<ProjectDetailed>(HttpMethod.Get, WithQuery("/v1/projects/retrieve", ("name", name)), options: options);

    public Task UpdateProjectAsync(string id, UpdateProjectRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/projects/{id}", request, options);

    public Task DeleteProjectByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Delete, $"/v1/projects/{id}", options: options);

    public Task DeleteProjectsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/projects/delete", new { ids }, options);

    public Task<FeedbackScoreNames> FindFeedbackScoreNamesByProjectIdsAsync(string? projectIds = null, RequestOptions? options = null)
        => Transport.SendAsync<FeedbackScoreNames>(HttpMethod.Get, WithQuery("/v1/projects/feedback-scores/names", ("projectIds", projectIds)), options: options);

    public Task<ProjectMetricResponsePublic> GetProjectMetricsAsync(string id, GetProjectMetricsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<ProjectMetricResponsePublic>(HttpMethod.Post, $"/v1/projects/{id}/metrics", request, options);

    public Task<ProjectStatsSummary> GetProjectStatsAsync(GetProjectStatsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<ProjectStatsSummary>(HttpMethod.Post, "/v1/projects/stats", request, options);
}
