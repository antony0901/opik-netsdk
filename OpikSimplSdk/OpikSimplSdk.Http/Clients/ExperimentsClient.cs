using System.Text.Json;
using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Http.Clients;

internal sealed class ExperimentsClient : ClientBase, IExperimentsClient
{
    private const int ExperimentItemsBulkMaxBytes = 4 * 1024 * 1024;

    public ExperimentsClient(IOpikHttpTransport transport) : base(transport)
    {
    }

    public Task<ExperimentPagePublic> FindExperimentsAsync(FindExperimentsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<ExperimentPagePublic>(HttpMethod.Post, "/v1/experiments/find", request, options);

    public Task CreateExperimentAsync(CreateExperimentRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/experiments", request, options);

    public Task<ExperimentPublic> GetExperimentByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync<ExperimentPublic>(HttpMethod.Get, $"/v1/experiments/{id}", options: options);

    public Task UpdateExperimentAsync(string id, UpdateExperimentRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/experiments/{id}", request, options);

    public Task DeleteExperimentsByIdAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/experiments/delete", new { ids }, options);

    public Task CreateExperimentItemsAsync(IEnumerable<ExperimentItem> items, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/experiment-items", items, options);

    public Task<ExperimentItemPublic> GetExperimentItemByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync<ExperimentItemPublic>(HttpMethod.Get, $"/v1/experiment-items/{id}", options: options);

    public Task DeleteExperimentItemsAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/experiment-items/delete", new { ids }, options);

    public Task ExperimentItemsBulkAsync(ExperimentItemsBulkRequest request, RequestOptions? options = null)
    {
        var payloadSize = JsonSerializer.SerializeToUtf8Bytes(request, OpikJson.Default).Length;
        if (payloadSize > ExperimentItemsBulkMaxBytes)
        {
            throw new ArgumentException($"ExperimentItemsBulk request exceeds 4 MB limit ({payloadSize} bytes).", nameof(request));
        }

        return Transport.SendAsync(HttpMethod.Post, "/v1/experiment-items/bulk", request, options);
    }

    public IAsyncEnumerable<byte[]> StreamExperimentItemsAsync(string experimentName, int? limit = null, string? lastRetrievedId = null, bool? truncate = null, RequestOptions? options = null)
        => Transport.StreamBytesAsync(HttpMethod.Get, WithQuery("/v1/experiment-items/stream", ("experimentName", experimentName), ("limit", limit), ("lastRetrievedId", lastRetrievedId), ("truncate", truncate)), options: options);

    public IAsyncEnumerable<byte[]> StreamExperimentsAsync(string name, int? limit = null, string? lastRetrievedId = null, RequestOptions? options = null)
        => Transport.StreamBytesAsync(HttpMethod.Get, WithQuery("/v1/experiments/stream", ("name", name), ("limit", limit), ("lastRetrievedId", lastRetrievedId)), options: options);

    public Task<IList<string>> FindFeedbackScoreNamesAsync(string? experimentIds = null, RequestOptions? options = null)
        => Transport.SendAsync<IList<string>>(HttpMethod.Get, WithQuery("/v1/experiments/feedback-scores/names", ("experimentIds", experimentIds)), options: options);

    public Task<ExperimentGroupResponse> FindExperimentGroupsAsync(FindExperimentGroupsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<ExperimentGroupResponse>(HttpMethod.Post, "/v1/experiments/groups/find", request, options);

    public Task<ExperimentGroupAggregationsResponse> FindExperimentGroupsAggregationsAsync(FindExperimentGroupsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<ExperimentGroupAggregationsResponse>(HttpMethod.Post, "/v1/experiments/groups/aggregations", request, options);
}
